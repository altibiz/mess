# Mess

Measure Event Store (Something).

## Development

Please install dependencies via the provided links if you don't know how to
install them and follow the setup instructions afterwards to run Mess.

### Dependencies

- [bash](https://www.gnu.org/software/bash/) (available through `git` on
  Windows)
- [docker-compose](https://www.docker.com/)
- [dotnet](https://dotnet.microsoft.com/en-us/)
- [yarn](https://yarnpkg.com/)

### Setup

1. Copy all secrets (`.env`, `secrets.json`, `secrets.sh`)
2. Run the [prepare](./scripts/prepare.sh) script with `bash`
3. Run the [watch](./scripts/watch.sh) script with `bash`
4. Open [Mess](https://localhost:3001)

### Timeseries migrations

When changing the timeseries model in any of the relevant projects run:

```bash
bash ./scripts/migrations.sh "<PROJECT_NAME>" "<MIGRATION_NAME>"
```

### Known issues

- [ ] Custom Startup classes for Xunit.DependencyInjection don't work
      properly
