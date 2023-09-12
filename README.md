# Mess

Measurement Event Store (Something).

Mess is a web application designed to accumulate measurements and metric data
from various sources and display it in an easily digestible web interface. It
leverages the event store pattern to handle data via push and pull mechanisms,
providing a central hub for all your metric data needs.

## Development

Please install dependencies via the provided links if you don't know how to
install them and follow the setup instructions afterwards to run Mess.

### Dependencies

- [node@20.4.0](https://nodejs.org/dist/v20.4.0/)
- [dotnet@^7.0.200](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.3/7.0.3.md)
- [docker@^19.03.0](https://docs.docker.com/engine/install/)

### Setup

1. `./mess prepare`
2. `./mess watch`
3. Open [Mess](https://localhost:5001)

### Dependency updates

Relevant files are:

- `.nvmrc`
- `package.json`
- `global.json`
- `Directory.Build.props`
- `docker-compose.yml`
- `.vscode/launch.json`

### Ignoring

Relevant files are:

- `.gitignore`
- `.prettierignore`
