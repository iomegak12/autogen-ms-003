from teams import customer_support_team
import asyncio


async def main(query):
    response = await customer_support_team.run(task=query)

    result = response.messages[-1].content

    if result.endswith("TERMINATE"):
        result = result.removesuffix("TERMINATE").strip()

    return result


if __name__ == "__main__":
    print(
        asyncio.run(main("What is the price of Master 3S Mouse?"))
    )
