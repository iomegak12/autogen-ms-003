using System.Text.Json;
using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using Azure.AI.Inference;
using FluentAssertions;
using Json.Schema;
using Json.Schema.Generation;

namespace AgenticAI;

public static class StructuredOutputDemo
{
    public static async Task RunAsync()
    {
        var deploymentName = LLMConfiguration.GetDeploymentName();
        var client = LLMConfiguration.GetAzureOpenAIClient();
        var systemPrompt = @"
            You are a helpful assistant.     
        ";

        var schemaBuilder = new JsonSchemaBuilder().FromType<Person>();
        var schema = schemaBuilder.Build();

        var agent = new OpenAIChatAgent(
            chatClient: client.GetChatClient(deploymentName),
            systemMessage: systemPrompt,
            name: "StructuredOutputAgent",
            temperature: 0.0f
        )
            .RegisterMessageConnector()
            .RegisterPrintMessage();

        var promptMessage = @"
            My name is John Doe, I am 30 years old, I work as a Software Engineer, 
            I live in San Francisco, and I love programming, hiking, and reading books.
            These days, started liking Space Tourism.
            Please provide my details in a structured format.";

        var prompt = new TextMessage(Role.User, promptMessage);
        var reply = await agent.GenerateReplyAsync(
            messages: [prompt],
            options: new GenerateReplyOptions
            {
                OutputSchema = schema
            }
        );

        var person = JsonSerializer.Deserialize<Person>(reply.GetContent()!);

        person.Should().NotBeNull();
        person!.Name.Should().Be("John Doe");

        Console.WriteLine($"Complete Person Details: {person}");
        Console.WriteLine("Program Successfully executed with structured output.");
    }
}