# AgenticAI

AgenticAI is a C# .NET 8.0 console application template designed for agentic AI practices. This repository provides a clean starting point for building .NET-based AI solutions, with best practices for configuration, testing, and containerization.

## Features
- .NET 8.0 SDK
- Console application entry point
- xUnit test project for unit testing
- Dockerfile and docker-compose for containerization
- GitHub Actions workflow for CI/CD (build & push Docker image)
- MIT License
- All configuration and secrets in `appsettings.json`

## Project Structure
```
AgenticAI.sln                # Solution file
AgenticAI/                   # Main console application
AgenticAI.Tests/             # xUnit test project
LICENSE                      # MIT License
README.md                    # Project documentation
.gitignore                   # Git ignore rules
Dockerfile                   # Docker build file
appsettings.json             # Configuration and secrets
.dockerignore                # Docker ignore rules
docker-compose.yml           # Docker Compose file
.github/workflows/           # GitHub Actions workflows
```

## Configuration
All secrets and configuration values are stored in `appsettings.json`. Example:
```json
{
  "AZUREOPENAI_API_KEY": "your-azure-openai-api-key-here"
}
```
**Never commit real secrets to version control. Use environment variables or secret managers in production.**

## Build & Run
### Prerequisites
- .NET 8.0 SDK
- Docker (for containerization)

### Build
```sh
dotnet build
```

### Run
```sh
dotnet run --project AgenticAI
```

### Test
```sh
dotnet test
```

### Docker
Build and run the Docker image:
```sh
docker build -t agenticai .
docker run --rm -it agenticai
```

Or use Docker Compose:
```sh
docker-compose up --build
```

## CI/CD
A GitHub Actions workflow is provided to build and push the Docker image on every push to `main`.

## License
MIT
