using AutoGen.Core;
using AutoGen.SemanticKernel;
using AutoGen.SemanticKernel.Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Web;

namespace AgenticAI;

#pragma warning disable SKEXP0050

public static class BraveSearchSKPluginInAutoGenMiddlewareDemo
{
    public static async Task RunAsync()
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
        var skAgent = new SemanticKernelAgent(
            kernel: kernel,
            name: "BraveSearchSKPluginInAutoGenMiddlewareDemo",
            systemMessage: "You are a helpful assistant that can search the web using Brave Search."
        )
            .RegisterMessageConnector()
            .RegisterPrintMessage();

        var reply = await skAgent.SendAsync(
            @"Tell me about the **latest advancements in Solar Energy in the year 2025**.
            Summarize the results in a concise manner about top 5 advancements."
        );

        Console.WriteLine($"Reply: {reply.GetContent()}");
        Console.WriteLine("Application completed successfully.");
    }
}