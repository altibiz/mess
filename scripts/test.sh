#!/usr/bin/env sh

export ASPNETCORE_ENVIRONMENT=Development
export DOTNET_ENVIRONMENT=Development
export ORCHARD_VERSION="1.5.0"

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
