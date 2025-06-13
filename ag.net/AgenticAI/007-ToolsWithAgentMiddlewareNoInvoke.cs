using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using Azure.AI.Inference;
using FluentAssertions;

namespace AgenticAI;

public static class ToolsWithAgentMiddlewareNoInvoke
{
    public static async Task RunAsync()
    {
        var deploymentName = LLMConfiguration.GetDeploymentName();
        var client = LLMConfiguration.GetAzureOpenAIClient();
        var systemPrompt = @"
            You are a helpful assistant that can perform various tasks using tools.     
        ";
        var tools = new Tools();
        var toolsCallingMiddleware = new FunctionCallMiddleware(
            functions:
            [
                tools.UpperCaseFunctionContract,
                tools.ConcatFunctionContract,
                tools.CalculateTaxFunctionContract
            ]
        );

        var agent = new OpenAIChatAgent(
            chatClient: client.GetChatClient(deploymentName),
            systemMessage: systemPrompt,
            name: "AgentWithTools",
            temperature: 0.0f
        )
            .RegisterMessageConnector()
            .RegisterMiddleware(toolsCallingMiddleware)
            .RegisterPrintMessage();

        var reply = await agent.SendAsync("Can you calculate the tax on 1000 at a rate of 5% and also on 2000 at a rate of 10%?");

        Console.WriteLine($"Agent Reply: {reply.GetContent()}");

        var toolCalls = reply.GetToolCalls();

        foreach (var toolCall in toolCalls!)
        {
            Console.WriteLine($"Tool Call: {toolCall.FunctionName}");
            Console.WriteLine($"Arguments: {toolCall.FunctionArguments}");
        }

        Console.WriteLine("Program Successfully executed with tools and agent middleware.");
    }
}