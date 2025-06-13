using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using FluentAssertions;
using OpenAI.Chat;

namespace AgenticAI;

public static class AgentWithRAGMiddleware
{
    public static async Task RunAsync()
    {
        var deploymentName = LLMConfiguration.GetDeploymentName();
        var client = LLMConfiguration.GetAzureOpenAIClient();
        var systemMessage = @"You are an AI assistant connected to Azure OpenAI.";

        var agent = new OpenAIChatAgent(
            chatClient: client.GetChatClient(deploymentName),
            name: "Azure OpenAI Agent",
            systemMessage: systemMessage
        )
            .RegisterMessageConnector()
            .RegisterMiddleware(
                async (messages, option, agent, cancellationToken) =>
                {
                    var today = DateTime.UtcNow;
                    var todayMessage = new TextMessage(
                        content: $"Today is {today:MMMM dd, yyyy}.",
                        role: Role.System
                    );

                    messages = messages.Concat([todayMessage]);

                    return await agent.GenerateReplyAsync(messages, option, cancellationToken);
                }
            )
            .RegisterPrintMessage();

        var reply = await agent.SendAsync("What is the date today?");

        Console.WriteLine($"Reply: {reply.GetContent()}");

        reply.Should().BeOfType<TextMessage>();

        Console.WriteLine("Agent with RAG middleware executed successfully.");
    }
}