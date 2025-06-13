using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using FluentAssertions;

namespace AgenticAI;

public static class MultimodalityModelDemo
{
    public static async Task RunAsync(string fileName = "")
    {
        var deploymentName = LLMConfiguration.GetDeploymentName();
        var client = LLMConfiguration.GetAzureOpenAIClient();
        var systemPrompt = @"
            You are a helpful assistant. 
            As an analyst, you try to extract information and analyze insights from the provided text and images. 
            You can process both text and images.
        ";

        var agent = new OpenAIChatAgent(
            chatClient: client.GetChatClient(deploymentName),
            systemMessage: systemPrompt,
            name: "MultimodalityModelAgent",
            temperature: 0.0f
        )
            .RegisterMessageConnector()
            .RegisterPrintMessage();

        var imageFileName = string.IsNullOrEmpty(fileName) ?
            "background.png" :
            fileName;

        var imagePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Resources",
            "Images",
            imageFileName
        );

        Console.WriteLine($"Image Path: {imagePath}");

        var imageBytes = await File.ReadAllBytesAsync(imagePath);
        var imageMessage = new ImageMessage(
            role: Role.User,
            BinaryData.FromBytes(imageBytes, "image/png"));

        var textMessage = new TextMessage(
            Role.User,
            "What insights can you provide from the image? Please analyze the content and provide a summary.");

        var multimodalMessage = new MultiModalMessage(Role.User, [textMessage, imageMessage]);

        var reply = await agent.SendAsync(multimodalMessage);

        reply.Should().NotBeNull();
        reply.Should().BeOfType<TextMessage>();

        Console.WriteLine($"Reply: {reply.GetContent()}");
        Console.WriteLine("Program Successfully executed with multimodal input.");
    }
}