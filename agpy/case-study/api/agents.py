from autogen_agentchat.agents import AssistantAgent
from services import product_inquiry_tool, order_placement_tool, \
    order_status_tool, complaint_registration_tool
from models import model_client


planning_system_message = """
You are a planning agent for an e-commerce use case.
Your job is to identify customer requests and delegate them to the correct agent.
Available Agents:
- Product Inquiry Agent: Handles product inquiries and provides information about products.
- Order Placement Agent: Handles order placement requests and manages the order process.
- Order Status Agent: Handles order status inquiries and provides updates on order status.
- Complaint Registration Agent: Handles customer complaints and manages complaint resolution.
- Response Agent: Responsible for generating final responses to the customer based on the information provided by other agents.

Once a task is completed by another agent, delegate the response to the **ResponseAgent** to format and 
send the final response to the customer.

If **ResponseAgent** responds with "TERMINATE", end the conversation.
"""

planning_agent = AssistantAgent(
    "PlanningAgent",
    description="Planning Agent for e-commerce use case",
    model_client=model_client,
    system_message=planning_system_message
)

product_inquiry_agent = AssistantAgent(
    "ProductInquiryAgent",
    description="Handles product inquiries and provides information about products.",
    model_client=model_client,
    system_message="""
        You are a Product Inquiry Agent. 
        Your job is to handle product inquiries and provide information about products.
    """,
    tools=[product_inquiry_tool]
)

order_placement_agent = AssistantAgent(
    "OrderPlacementAgent",
    description="Handles order placement requests and manages the order process.",
    model_client=model_client,
    system_message="""
        You are an Order Placement Agent. 
        Your job is to handle order placement requests and manage the order process.
    """,
    tools=[order_placement_tool]
)

order_status_agent = AssistantAgent(
    "OrderStatusAgent",
    description="Handles order status inquiries and provides updates on order status.",
    model_client=model_client,
    system_message="""
        You are an Order Status Agent. 
        Your job is to handle order status inquiries and provide updates on order status.
    """,
    tools=[order_status_tool]
)

complaint_registration_agent = AssistantAgent(
    "ComplaintRegistrationAgent",
    description="Handles customer complaints and manages complaint resolution.",
    model_client=model_client,
    system_message="""
        You are a Complaint Registration Agent. 
        Your job is to handle customer complaints and manage complaint resolution.
    """,
    tools=[complaint_registration_tool]
)

response_agent = AssistantAgent(
    "ResponseAgent",
    description="Responsible for generating final responses to the customer based on the information provided by other agents.",
    model_client=model_client,
    system_message="""
        Your job is to format the response provided by other agents and send the final response in a friendly manner to the customer,
        Always end the conversation with 'TERMINATE'.
        
        Examples:
        - If an order is placed, confirm it and include the Order ID in the response.
        - If a product inquiry is made, provide the summary of the product details.
        - If an order status is requested, provide the current status of the order.
        - If a complaint is registered, acknowledge it and provide the complaint ID.
        - If the customer asks for help, provide a friendly response and ask if they need further assistance.
    """
)
