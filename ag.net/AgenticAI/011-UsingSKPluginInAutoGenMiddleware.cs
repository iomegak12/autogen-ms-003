using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using AutoGen.SemanticKernel;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;

namespace AgenticAI;

public static class UsingSKPluginInAutoGenMiddleware
{
    public static async Task RunAsync()
    {
        Console.WriteLine("Using SK Plugin in AutoGen Middleware");

        var kernel = Kernel.CreateBuilder()
           .AddAzureOpenAIChatCompletion(
               Utils.GetConfigurationValue("AzureOpenAI:DeploymentName"),
               Utils.GetConfigurationValue("AzureOpenAI:Endpoint"),
               Utils.GetConfigurationValue("AzureOpenAI:ApiKey"))
           .Build();

        var getWeatherMethod = KernelFunctionFactory.CreateFromMethod(
            method: (string city) =>
            {
                return Task.FromResult($"The weather in {city} is sunny with a high of 25°C.");
            },
            functionName: "GetWeather",
            description: "Get the current weather for a specified city."
        );

        var getWeatherPlugin = kernel.CreatePluginFromFunctions("my_plgin", [getWeatherMethod]);
        var kernelPluginMiddleware = new KernelPluginMiddleware(kernel, getWeatherPlugin);
        var deploymentName = LLMConfiguration.GetDeploymentName();
        var client = LLMConfiguration.GetAzureOpenAIClient();
        var systemPrompt = @"
            You're a helpful assistant that can answer questions about the weather.
            You can use the GetWeather function to get the current weather for a specified city.";

        var agent = new OpenAIChatAgent(
            chatClient: client.GetChatClient(deploymentName),
            name: "WeatherAgent",
            temperature: 0.0f,
            systemMessage: systemPrompt
        )
            .RegisterMessageConnector()
            .RegisterMiddleware(kernelPluginMiddleware)
            .RegisterPrintMessage();

        var toolAggregatedResult = await agent.SendAsync("What is the weather in Paris?");

        Console.WriteLine($"Tool Aggregated Result: {toolAggregatedResult.GetContent()}");
        Console.WriteLine("Application finished. Press any key to exit.");
    }
}