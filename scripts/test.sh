#!/usr/bin/env bash
set -o pipefail

abs() { echo "$(cd "$(dirname "$1")" && pwd)/$(basename "$1")"; }
SCRIPT_DIR="$(dirname "$(abs "$0")")"
ROOT_DIR="$(dirname "$SCRIPT_DIR")"

# NOTE: this is messing it up somehow
# export ASPNETCORE_ENVIRONMENT=Development
# export DOTNET_ENVIRONMENT=Development
export ORCHARD_APP_DATA="$ROOT_DIR/App_Data"

kill_services() {
  if [ "$E2E_SERVER_PID" ]; then
    printf "[Mess] Killing E2E server with PID '%s'...\n" "$E2E_SERVER_PID"
    kill -s SIGKILL "$E2E_SERVER_PID"
  fi
  printf "\n"

  if [ ! "$CI" ] && [ ! "$1" ]; then
    printf "[Mess] Running 'docker-compose down'...\n"
    docker-compose down
  fi
}
trap 'kill_services $1' SIGINT

if [ ! "$CI" ] && [ ! "$1" ]; then
  printf "[Mess] Running 'docker-compose up --detach'...\n"
  docker-compose up --detach || exit 1
fi
printf "\n"

if [ ! "$1" ]; then
  printf "[Mess] Running E2E server with 'dotnet'...\n"
  dotnet run \
    --configuration Release \
    --property:consoleLoggerParameters=ErrorsOnly \
    --project "$ROOT_DIR/src/Mess.Web/Mess.Web.csproj" || exit 1 &
  E2E_SERVER_PID=$!
  printf "\n"

  printf "[Mess] Waiting for E2E server to start...\n"
  until curl --silent --fail http://localhost:5000/admin; do
    printf "[Mess] E2E server not ready yet, waiting 1 second...\n"
    sleep 1
  done
  printf "\n"
fi

printf "[Mess] Testing with 'dotnet'...\n"
if [ "$1" ]; then
  dotnet test --filter "FullyQualifiedName~$1"
else
  dotnet test
fi
EXIT_CODE=$?
printf "\n"

kill_services "$1"

exit $EXIT_CODE
