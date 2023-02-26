#!/usr/bin/env bash
set -eo pipefail

abs() { echo "$(cd "$(dirname "$1")" && pwd)/$(basename "$1")"; }
SCRIPT_DIR="$(dirname "$(abs "$0")")"
ROOT_DIR="$(dirname "$SCRIPT_DIR")"

export ASPNETCORE_ENVIRONMENT=Development
export DOTNET_ENVIRONMENT=Development
export ORCHARD_APP_DATA="$ROOT_DIR/App_Data"

export NODE_OPTIONS="--no-warnings"
export NODE_ENV="development"
export MESS_CLIENT_OUTPUT_DIRS="$ROOT_DIR/src/Mess.Client/dist"

stop() {
  printf "\n"
  printf "[Mess] Giving everything 1 second to stop properly...\n"
  sleep 1s
  printf "\n"
}
trap 'stop' SIGINT

printf "[Mess] Running 'docker-compose up' and 'dotnet'...\n"
printf "
docker-compose up; \

dotnet run \
  --configuration Debug \
  --property:consoleLoggerParameters=ErrorsOnly \
  --project '%s/src/Mess.Web/Mess.Web.csproj'; \

" "$ROOT_DIR" | xargs -o -P2 -IR /usr/bin/env bash -c R
printf "\n"
