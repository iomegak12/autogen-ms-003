using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;

namespace AgenticAI;

public static class ConnectWithAzureOpenAI
{
    public static async Task RunAsync()
    {
        var deploymentName = LLMConfiguration.GetDeploymentName();
        var client = LLMConfiguration.GetAzureOpenAIClient();
        var systemMessage = @"You are an AI assistant connected to Azure OpenAI. 
        Your task is to assist users with their queries using the Azure OpenAI service.";

        var agent = new OpenAIChatAgent(
            chatClient: client.GetChatClient(deploymentName),
            name: "Azure OpenAI Agent",
            systemMessage: systemMessage
        )
            .RegisterMessageConnector()
            .RegisterPrintMessage();

        var prompt = @"Can you write a piece of C# code that calculates 100th fibonacci number?";

        await agent.SendAsync(prompt);
    }
}