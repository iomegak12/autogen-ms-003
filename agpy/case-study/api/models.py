import os

from dotenv import load_dotenv
from autogen_ext.models.openai import AzureOpenAIChatCompletionClient, OpenAIChatCompletionClient

load_dotenv()

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

# model_client = OpenAIChatCompletionClient(
#     model="gpt-4o",
#     api_key=os.environ.get("OPENAI_API_KEY"),
# )
