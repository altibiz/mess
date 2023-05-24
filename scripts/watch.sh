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

stop() {
  printf "\n"
  printf "[Mess] Giving everything 1 second to stop properly...\n"
  sleep 1s
  printf "\n"
}
trap 'stop' SIGINT

printf "[Mess] Running 'docker-compose up', 'yarn' and 'dotnet'...\n"
printf "
docker-compose up; \

yarn watch; \

dotnet watch run \
  --configuration Debug \
  --property:consoleLoggerParameters=ErrorsOnly \
  --project '%s/src/Mess.Web/Mess.Web.csproj'; \

" "$ROOT_DIR" | xargs -o -P3 -IR /usr/bin/env bash -c R
printf "\n"
