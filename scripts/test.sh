#!/usr/bin/env bash
set -o pipefail

abs() { echo "$(cd "$(dirname "$1")" && pwd)/$(basename "$1")"; }
SCRIPT_DIR="$(dirname "$(abs "$0")")"
ROOT_DIR="$(dirname "$SCRIPT_DIR")"

export ASPNETCORE_ENVIRONMENT=Development
export DOTNET_ENVIRONMENT=Development
export ORCHARD_APP_DATA="$ROOT_DIR/App_Data/run"

kill_services() {
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

printf "[Mess] Testing with 'dotnet'...\n"
dotnet test "$@"
EXIT_CODE=$?
printf "\n"

kill_services "$1"

exit $EXIT_CODE
