#!/usr/bin/env bash
set -eo pipefail

export ASPNETCORE_ENVIRONMENT=Development
export DOTNET_ENVIRONMENT=Development

printf "[Mess] Running 'docker-compose up -d'...\n"
if [ "$CI" ]; then
  docker-compose up -d --env-file "$ROOT_DIR/ci.env"
else
  docker-compose up -d
fi
printf "\n"

printf "[Mess] Tesing with 'dotnet'...\n"
if [ "$1" ]; then
  dotnet test \
    --configuration Debug \
    --filter "FullyQualifiedName~$1"
else
  dotnet test \
    --configuration Debug
fi
printf "\n"

if [ "$CI" ]; then
  docker-compose down --env-file "$ROOT_DIR/ci.env"
else
  printf "[Mess] Don't forget to run 'docker-compose down' when you stop testing\n"
  printf "\n"
fi
