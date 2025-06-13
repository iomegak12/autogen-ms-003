using Azure.AI.OpenAI;

namespace AgenticAI;

public static class LLMConfiguration
{
    public static AzureOpenAIClient GetAzureOpenAIClient()
    {
        var endpoint = Utils.GetConfigurationValue("AzureOpenAI:Endpoint");
        var apiKey = Utils.GetConfigurationValue("AzureOpenAI:ApiKey");

        if (string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException("Azure OpenAI configuration is not set properly.");
        }

        var client = new AzureOpenAIClient(
            new Uri(endpoint),
            new System.ClientModel.ApiKeyCredential(apiKey)
        );

        return client;
    }

    public static string GetDeploymentName()
    {
        var deploymentName = Utils.GetConfigurationValue("AzureOpenAI:DeploymentName");

        if (string.IsNullOrEmpty(deploymentName))
        {
            throw new InvalidOperationException("Deployment name is not set in the configuration.");
        }

        return deploymentName;
    }
}