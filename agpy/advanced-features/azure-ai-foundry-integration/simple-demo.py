from azure.ai.projects import AIProjectClient
from azure.identity import DefaultAzureCredential
from azure.ai.agents.models import ListSortOrder

project = AIProjectClient(
    credential=DefaultAzureCredential(),
    endpoint="https://ramkumarazureaifoundryv10.services.ai.azure.com/api/projects/jdrazureaifprojectv10")

agent = project.agents.get_agent("asst_4vDgE6CXViE504cIZva3ZZNT")

thread = project.agents.threads.get("thread_UtONGtiwezjwQL8d2VXb1Sgk")

message = project.agents.messages.create(
    thread_id=thread.id,
    role="user",
    content="what's microsoft's annual leave policy, please give citation about your answers"
)

run = project.agents.runs.create_and_process(
    thread_id=thread.id,
    agent_id=agent.id)

if run.status == "failed":
    print(f"Run failed: {run.last_error}")
else:
    messages = project.agents.messages.list(thread_id=thread.id, order=ListSortOrder.ASCENDING)

    for message in messages:
        if message.text_messages:
            print(f"{message.role}: {message.text_messages[-1].text.value}")