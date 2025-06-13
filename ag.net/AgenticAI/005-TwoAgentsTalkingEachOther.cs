using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using FluentAssertions;

namespace AgenticAI;

public static class TwoAgentsTalkingEachOther
{
    public static async Task RunAsync()
    {
        var deploymentName = LLMConfiguration.GetDeploymentName();
        var client = LLMConfiguration.GetAzureOpenAIClient();
        var systemPrompt = @"
            You're a Helpful School Teacher AI, who creates elementary school math problems for students and Checks their answers.        
            If the answer is correct, you say 'Correct' and stop the conversation by saying 'COMPLETE'.         

            If the answer is wrong, you say 'Incorrect!, and ask the student to fix it. 
        ";

        var teacherAgent = new OpenAIChatAgent(
            chatClient: client.GetChatClient(deploymentName),
            name: "TeacherAgent",
            temperature: 1.0f,
            systemMessage: systemPrompt
        )
            .RegisterMessageConnector()
            .RegisterMiddleware(
                async (messages, option, agent, cancellationToken) =>
                {
                    var reply = await agent.GenerateReplyAsync(messages, option);

                    if (reply.GetContent()?.ToLower().Contains("complete") is true)
                    {
                        Console.WriteLine("Teacher Agent has completed the task.");

                        return new TextMessage(
                            Role.Assistant,
                            GroupChatExtension.TERMINATE,
                            from: reply.From
                        );
                    }

                    return reply;
                }
            )
            .RegisterPrintMessage();

        var studentAgent = new OpenAIChatAgent(
            chatClient: client.GetChatClient(deploymentName),
            name: "StudentAgent",
            temperature: 1.0f,
            systemMessage: @"
                You're a Student AI, who tries to solve elementary school math problems given by the Teacher AI.
                You will try to answer the question, and if you get it wrong, 
                you will try again until you get it right.
                If you get it right, you will say 'I got it right!'.
            "
        )
            .RegisterMessageConnector()
            .RegisterPrintMessage();

        var conversation = await studentAgent.InitiateChatAsync(
            receiver: teacherAgent,
            message: @"Hello Teacher!, please create a math problem for me to solve.",
            maxRound: 10
        );

        Console.WriteLine("Conversation ended.");

        foreach (var message in conversation)
        {
            Console.WriteLine($"{message.From}: {message.GetContent()}");
        }

        conversation.Count().Should().BeLessThan(15);
        conversation.Last().IsGroupChatTerminateMessage().Should().BeTrue();

        Console.WriteLine("Application Completed Successfully.");
    }
}