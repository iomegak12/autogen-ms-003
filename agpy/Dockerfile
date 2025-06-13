# syntax=docker/dockerfile:1
FROM python:3.11-slim

WORKDIR /app

# Install uv
RUN pip install --no-cache-dir uv

# Copy requirements and install dependencies
COPY requirements.txt .
RUN uv pip install -r requirements.txt

# Copy source code and other folders
COPY src/ src/
COPY .env .
COPY logs/ logs/
COPY data/ data/
COPY uploads/ uploads/

CMD ["uv", "pip", "run", "src/main.py"]
