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

- Custom Startup classes for Xunit.DependencyInjection don't work properly
- Source maps are present in production builds
- Webpack generates an empty js file alongside css files which is annoying
- Modules that should exist:
  - [ ] Mess.EntityFrameworkCore
        (with Mess.EntityFrameworkCore.Postgres feature)
  - [ ] Mess.Mqtt
  - [ ] Mess.Modbus
- Tenants in tests don't mean the same thing as tenants in server
- Tests shouldn't care about tenants there should be no tenant fixture
- E2E Tests should be able to run somewhat in parallel
- scripts should be in typescript and we should get rid of bash dependency
- Mess.System should be top level project
- Mess.Tenants shouldn't be a thing
- Mess.OrchardCore should be a top-level project
- we should only use Newtonsoft.Json for JSON serialization
- Mess.System.Test should be a top-level test project
- Mess.OrchardCore.Test should be a top-level test project
