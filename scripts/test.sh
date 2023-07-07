#!/usr/bin/env bash
set -o pipefail

export ASPNETCORE_ENVIRONMENT=Development
export DOTNET_ENVIRONMENT=Development

start_services() {
  if [ ! "$CI" ] && [ ! "$1" ]; then
    printf "[Mess] Running 'docker-compose up --detach'...\n"
    docker-compose up --detach || exit 1
    printf "\n"
  fi
}
kill_services() {
  if [ ! "$CI" ] && [ ! "$1" ]; then
    printf "[Mess] Running 'docker-compose down'...\n"
    docker-compose down
    printf "\n"
  fi
}

start_services "$1"
trap 'kill_services $1' SIGINT

printf "[Mess] Testing with 'dotnet'...\n"
dotnet test "$@"
EXIT_CODE=$?
printf "\n"

kill_services "$1"

exit $EXIT_CODE
