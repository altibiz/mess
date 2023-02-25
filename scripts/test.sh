#!/usr/bin/env bash
set -eo pipefail

# NOTE: this is messing it up somehow
# export ASPNETCORE_ENVIRONMENT=Development
# export DOTNET_ENVIRONMENT=Development

if [ ! "$CI" ]; then
  printf "[Mess] Running 'docker-compose up --detach'...\n"
  docker-compose up --detach
fi
printf "\n"

printf "[Mess] Tesing with 'dotnet'...\n"
if [ "$1" ]; then
  dotnet test --filter "FullyQualifiedName~$1"
else
  dotnet test
fi
printf "\n"

if [ ! "$CI" ]; then
  printf "[Mess] Running 'docker-compose down'...\n"
  docker-compose down
fi
