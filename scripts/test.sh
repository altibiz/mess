#!/usr/bin/env bash
set -euo pipefail

export ASPNETCORE_ENVIRONMENT=Development
export DOTNET_ENVIRONMENT=Development

printf "[Mess] Running 'docker-compose up -d'...\n"
docker-compose up -d
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

printf "[Mess] Don't forget to run 'docker-compose down' when you stop testing\n"
printf "\n"

# NOTE: just leave this here in case it becomes valuable in the future
# printf "[Mess] Do you want to stop docker with 'docker-compose down'? (y/n) "
# read -r yn
# printf "\n"
# case $yn in
#   [Yy])
#     printf "[Mess] Running 'docker-compose down'...\n"
#     docker-compose down
#     printf "\n "
#     ;;
# esac
