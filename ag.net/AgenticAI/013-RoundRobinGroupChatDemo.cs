using AutoGen;
using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using AutoGen.SemanticKernel;
using AutoGen.SemanticKernel.Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Web;

namespace AgenticAI;

#pragma warning disable SKEXP0050

public static class RoundRobinGroupChatDemo
{

    public static async Task<IAgent> CreateBraveSearchAgentAsync()
    {
        var config = Configuration.ConfigureAppSettings();
        var openAISettings = new OpenAIOptions();
        var pluginSettings = new PluginOptions();

        config.GetSection("OpenAI").Bind(openAISettings);
        config.GetSection("PluginConfig").Bind(pluginSettings);

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Warning);
            builder.AddConfiguration(config);
            builder.AddConsole();
        });

        var builder = Kernel.CreateBuilder();

        builder.Services.AddSingleton(loggerFactory);
        builder.AddChatCompletionService(openAISettings);
        builder.AddBraveConnector(pluginSettings, ApiLoggingLevel.ResponseAndRequest);
        builder.Plugins.AddFromType<WebSearchEnginePlugin>();

        var kernel = builder.Build();
        var systemPrompt = @"
            You are a helpful agent that can search the web using Brave Search.
            You put the original search results between ```brave and ```

            e.g.

            ```brave
            <search results>
            ```";

        var kernelAgent = new SemanticKernelAgent(
            kernel: kernel,
            name: "RoundRobinGroupChatDemo",
            systemMessage: systemPrompt
        )
            .RegisterMessageConnector()
            .RegisterPrintMessage();


        return kernelAgent;
    }

    public static async Task<IAgent> CreateSummarizeAgentAsync()
    {
        var deploymentName = LLMConfiguration.GetDeploymentName();
        var client = LLMConfiguration.GetAzureOpenAIClient();
        var systemPrompt = @"
            You are a  helpful agent, and you summarize the contents of the brave web search results.
            And return the summary in a single paragraph.
            Your summary should be concise and to the point.";

        var agent = new OpenAIChatAgent(
            chatClient: client.GetChatClient(deploymentName),
            name: "SummarizeAgent",
            systemMessage: systemPrompt,
            temperature: 0.2f
        )
            .RegisterMessageConnector()
            .RegisterPrintMessage();

        return agent;
    }

    public static async Task RunAsync()
    {
        var userProxyAgent = new UserProxyAgent(
            name: "UserProxyAgent",
            humanInputMode: HumanInputMode.ALWAYS)
            .RegisterPrintMessage();

        var braveSearchAgent = await CreateBraveSearchAgentAsync();
        var summarizeAgent = await CreateSummarizeAgentAsync();
        var agents = new IAgent[]
        {
            userProxyAgent,
            braveSearchAgent,
            summarizeAgent
        };

        var groupChat = new RoundRobinGroupChat(agents);
        var groupChatManager = new GroupChatManager(groupChat);

        var history = await userProxyAgent.InitiateChatAsync(
            receiver: groupChatManager,
            message: "How do you deploy OpenAI Resources in Azure?",
            maxRound: 10
        );

        Console.WriteLine("Group chat history:");

        foreach (var chatMessage in history)
        {
            Console.WriteLine($"{chatMessage.From}: {chatMessage.GetContent()}");
        }

        Console.WriteLine("Application completed successfully.");
    }
}