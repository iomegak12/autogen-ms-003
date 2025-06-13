using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using Azure.AI.Inference;
using FluentAssertions;

namespace AgenticAI;

public static class ToolsWithAgentMiddleware
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
            ],
            functionMap: new Dictionary<string, Func<string, Task<string>>>
            {
                { nameof(tools.UpperCase), tools.UpperCaseWrapper },
                { nameof(tools.Concat), tools.ConcatWrapper },
                { nameof(tools.CalculateTax), tools.CalculateTaxWrapper }
            }
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

        var reply = await agent.SendAsync("Can you convert the following text to uppercase? 'hello world'");

        Console.WriteLine($"Agent Reply: {reply.GetContent()}");

        reply.Should().BeOfType<ToolCallAggregateMessage>();
        reply?.GetToolCalls()?.First().FunctionName.Should().Be(nameof(tools.UpperCase));

        Console.WriteLine("*********************");

        Console.WriteLine("Testing #2");
        Console.WriteLine("Now let's test the Concat tool with a list of strings.");

        reply = await agent.SendAsync(
            "Can you concatenate these strings: 'Hello', 'World', 'from', 'Agent' with a comma separator?");

        Console.WriteLine($"Agent Reply: {reply.GetContent()}");

        reply.Should().BeOfType<ToolCallAggregateMessage>();

        Console.WriteLine("*********************");

        Console.WriteLine("Testing #3");
        Console.WriteLine("Now let's test the CalculateTax tool with an amount and tax rate.");

        reply = await agent.SendAsync(
            "Can you calculate the tax on 1000 at a rate of 5%?");

        Console.WriteLine($"Agent Reply: {reply.GetContent()}");
        reply.Should().BeOfType<ToolCallAggregateMessage>();

        Console.WriteLine("*********************");

        Console.WriteLine("Testing #4");
        Console.WriteLine("Now let's test the CalculateTax tool with multiple inputs.");

        reply = await agent.SendAsync(
            "Can you calculate the tax on 1000 at a rate of 5% and also on 2000 at a rate of 10%?");

        Console.WriteLine($"Agent Reply: {reply.GetContent()}");
        reply.Should().BeOfType<ToolCallAggregateMessage>();

        Console.WriteLine("*********************");


        Console.WriteLine("Program Successfully executed with tools and agent middleware.");
    }
}