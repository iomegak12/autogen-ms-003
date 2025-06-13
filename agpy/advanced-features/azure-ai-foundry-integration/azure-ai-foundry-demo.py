from autogen_core import CancellationToken
from autogen_ext.agents.azure import AzureAIAgent
from azure.ai.agents.models import BingGroundingTool
from azure.ai.projects.aio import AIProjectClient
from azure.identity.aio import DefaultAzureCredential
from autogen_core.messages import TextMessage   
from dotenv import load_dotenv

import os 

load_dotenv()

async def bing_grounding_demo():
    async with DefaultAzureCredential() as credential:
        async with AIProjectClient(
            credential=credential, 
            endpoint=os.getenv("PROJECT_CONNECTION_STRING","")) as project_client:
            connection = await project_client.connections.get(name="GroundingBingSearch")
            bing_tool = BingGroundingTool(connection.id)
            
            agent_with_bing_grounding = AzureAIAgent(
                name="BingGroundingAgent",
                description="An agent that uses Bing for grounding",
                project_client=project_client,
                deployment_name="gpt-4.1",
                instructions="You are a helpful assistant that uses Bing to find information.",
                tools = bing_tool.definitions,
                metadata={
                    "source": "AzureAIAgent"
                }
            )
            
            result = await agent_with_bing_grounding.on_messages(
                [
                    TextMessage(
                        content="what is microsoft's anual leave policy?",
                        source="user",
                    )
                ],
                cancellation_token=CancellationToken(),
                messagse_timeout=10
            )
            
            print(result)
            
            
import asyncio

if __name__ == "__main__":
    asyncio.run(bing_grounding_demo())