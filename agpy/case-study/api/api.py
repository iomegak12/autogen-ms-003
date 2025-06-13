from fastapi import FastAPI
from pydantic import BaseModel

import uvicorn
from main import main


class ChatRequest(BaseModel):
    query: str


app = FastAPI()


@app.post("/chat")
async def chat(request: ChatRequest):
    response = await main(request.query)

    return {
        "response": response
    }


if __name__ == "__main__":
    uvicorn.run(app,
                host="0.0.0.0.0",
                port=8000,
                log_level="info",
                reload=True,
                workers=1)
