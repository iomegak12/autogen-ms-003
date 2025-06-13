# agentic-ai

A Python project template for agentic AI workflows, containerized with Docker and ready for cloud-native development.

## Features
- MIT License
- Python 3.x
- Dependency management with [uv](https://github.com/astral-sh/uv)
- Virtual environment managed by uv
- All dependencies in `requirements.txt`
- Configurations and secrets in `.env`
- Docker and Docker Compose support
- GitHub Actions workflow for Docker image build
- Organized folder structure: `src/`, `logs/`, `data/`, `uploads/`, `docs/`
- Main entry point: `src/main.py` (displays a Welcome Message)

## Project Structure
```
agentic-ai/
├── .env
├── .gitignore
├── LICENSE
├── README.md
├── requirements.txt
├── Dockerfile
├── docker-compose.yml
├── .github/
│   └── workflows/
│       └── docker-image.yml
├── logs/
├── data/
├── uploads/
├── docs/
└── src/
    └── main.py
```

## Setup

### 1. Install [uv](https://github.com/astral-sh/uv)
```
pip install uv
```

### 2. Create a virtual environment and install dependencies
```
uv venv
uv pip install -r requirements.txt
```

### 3. Run the application
```
uv pip run src/main.py
```

### 4. Using Docker
Build and run the container:
```
docker compose up --build
```

## Configuration
- Edit `.env` for secrets and environment variables.

## Logging, Data, and Uploads
- Place logs in `logs/`
- Place data files in `data/`
- Place uploads in `uploads/`

## Documentation
- Add project documentation in `docs/`

---

## License
MIT
