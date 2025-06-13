import asyncio

from autogen_ext.models.openai import AzureOpenAIChatCompletionClient
from autogen_ext.tools.mcp import SseMcpToolAdapter, SseServerParams
from autogen_agentchat.agents import AssistantAgent
from autogen_agentchat.ui import Console
from autogen_core import CancellationToken
from dotenv import load_dotenv

import os

async def main() -> None:
    server_params = SseServerParams(
        url="http://localhost:8004/sse",
        headers={
            "Content-Type": "application/json",
        },
        timeout=30,
    )
    
    adapter = await SseMcpToolAdapter.from_server_params(server_params, "write_file")
    
    load_dotenv(override=True)

    azure_openai_api_key = os.getenv("AZURE_OPENAI_API_KEY")
    azure_openai_endpoint = os.getenv("AZURE_OPENAI_ENDPOINT")
    azure_openai_api_version = os.getenv("AZURE_OPENAI_API_VERSION", "2023-05-15")
    azure_openai_deployment_name = os.getenv(
        "AZURE_OPENAI_DEPLOYMENT_NAME", "gpt-35-turbo")

    if not azure_openai_api_key or \
            not azure_openai_endpoint or \
            not azure_openai_api_version or \
            not azure_openai_deployment_name:
        raise ValueError(
            "Please set Azure Open AI Endpoint Details in your environment variables.")

    model_client = AzureOpenAIChatCompletionClient(
        azure_deployment=azure_openai_deployment_name,
        model=azure_openai_deployment_name,
        api_key=azure_openai_api_key,
        azure_endpoint=azure_openai_endpoint,
        api_version=azure_openai_api_version,
    )
    
    agent = AssistantAgent(
        name="MCPIntegrationAgent",
        model_client=model_client,
        tools=[adapter],
        description="An agent that integrates with MCP to write files.",
    )
    
    await Console(
        agent.run_stream(
            task="Translate 'Hello, My Name is Ramkumar' to Spanish and write it to a file named '/greeting.txt'.",)
    )
    

if __name__ == "__main__":
    try:
        asyncio.run(main())
    except KeyboardInterrupt:
        print("Process interrupted by user.")
    except Exception as e:
        print(f"An error occurred: {e}")