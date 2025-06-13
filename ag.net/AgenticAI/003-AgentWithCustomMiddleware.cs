using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using FluentAssertions;
using OpenAI.Chat;

namespace AgenticAI;

public static class AgentWithCustomMiddleware
{
    public static async Task RunAsync()
    {
        var deploymentName = LLMConfiguration.GetDeploymentName();
        var client = LLMConfiguration.GetAzureOpenAIClient();
        var systemMessage = @"You are an AI assistant connected to Azure OpenAI.";
        var totalTokenCount = 0;

        var agent = new OpenAIChatAgent(
            chatClient: client.GetChatClient(deploymentName),
            name: "Azure OpenAI Agent",
            systemMessage: systemMessage
        )
            .RegisterMiddleware(
                async (messages, option, agent, cancellationToken) =>
                {
                    var reply = await agent.GenerateReplyAsync(messages, option, cancellationToken);

                    if (reply is MessageEnvelope<ChatCompletion> chatCompletion)
                    {
                        var tokenCount = chatCompletion.Content.Usage.TotalTokenCount;

                        totalTokenCount += tokenCount;

                        Console.WriteLine($"Total token count: {totalTokenCount}");
                    }

                    return reply;
                }
            )
            .RegisterMiddleware(new OpenAIChatRequestMessageConnector());

        var reply = await agent.SendAsync("Tell me a serious joke about Batman Joker.");

        Console.WriteLine($"Reply: {reply.GetContent()}");
        Console.WriteLine($"Total token count after reply: {totalTokenCount}");

        reply.Should().BeOfType<TextMessage>();
        totalTokenCount.Should().BeGreaterThan(0);
        
        Console.WriteLine("Agent with custom middleware executed successfully.");
    }
}