using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using FluentAssertions;

namespace AgenticAI;

public static class ConnectWithAzureOpenAIEval
{
    public static async Task RunAsync()
    {
        var deploymentName = LLMConfiguration.GetDeploymentName();
        var client = LLMConfiguration.GetAzureOpenAIClient();
        var systemMessage = @"You are an AI assistant connected to Azure OpenAI. 
        Your task is to convert what user said in uppercase.";

        var agent = new OpenAIChatAgent(
            chatClient: client.GetChatClient(deploymentName),
            name: "Azure OpenAI Agent",
            systemMessage: systemMessage
        )
            .RegisterMessageConnector()
            .RegisterPrintMessage();

        var prompt = @"Hello Microsoft!";
        var reply = await agent.SendAsync(prompt);

        reply.Should().NotBeNull();
        reply.Should().BeOfType<TextMessage>();
        reply.GetContent().Should().Be("HELLO MICROSOFT!");

        var conversationHistory = new List<IMessage>
        {
            new TextMessage(Role.User, "Hello World!"),
            new TextMessage(Role.Assistant, reply.GetContent()!),
        };

        reply = await agent.SendAsync("Hello World Again!", conversationHistory);

        reply.Should().NotBeNull();
        reply.Should().BeOfType<TextMessage>();
        reply.GetContent().Should().Be("HELLO WORLD AGAIN!");
    }
}