from autogen_agentchat.conditions import TextMentionTermination, MaxMessageTermination
from autogen_agentchat.teams import SelectorGroupChat
from agents import response_agent, planning_agent, product_inquiry_agent, \
    order_placement_agent, order_status_agent, complaint_registration_agent
from models import model_client
from typing import Sequence
from autogen_agentchat.messages import AgentEvent, ChatMessage

text_termination_condition = TextMentionTermination("TERMINATE")
max_message_termination_condition = MaxMessageTermination(10)
combined_termination_condition = text_termination_condition | max_message_termination_condition


def custom_selector_func(messages: Sequence[AgentEvent | ChatMessage]) -> str | None:
    if messages[-1].source != planning_agent.name:
        return planning_agent.name

    return None


customer_support_team = SelectorGroupChat(
    [
        planning_agent,
        product_inquiry_agent,
        order_placement_agent,
        order_status_agent,
        complaint_registration_agent,
        response_agent
    ],
    model_client=model_client,
    termination_condition=combined_termination_condition,
    allow_repeated_speaker=True,
    selector_func=custom_selector_func,
)
