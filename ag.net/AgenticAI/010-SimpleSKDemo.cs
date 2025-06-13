using AutoGen.Core;
using AutoGen.SemanticKernel;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;

namespace AgenticAI;

public static class SimpleSKDemo
{
    public static async Task RunAsync()
    {
        var kernel = Kernel.CreateBuilder()
            .AddAzureOpenAIChatCompletion(
                Utils.GetConfigurationValue("AzureOpenAI:DeploymentName"),
                Utils.GetConfigurationValue("AzureOpenAI:Endpoint"),
                Utils.GetConfigurationValue("AzureOpenAI:ApiKey"))
            .Build();

        var chatAgent = new ChatCompletionAgent()
        {
            Kernel = kernel,
            Name = "SimpleChatAgent",
            Description = "A simple chat agent that responds to user queries."
        };

        var skAgent = new SemanticKernelChatCompletionAgent(chatAgent)
            .RegisterMiddleware(new SemanticKernelChatMessageContentConnector())
            .RegisterPrintMessage();

        var reply = await skAgent.SendAsync("How do I able to use Semantic Kernel with AutoGen?");

        Console.WriteLine($"Agent Response: {reply.GetContent()}");
        Console.WriteLine("Application finished. Press any key to exit.");
    }
}