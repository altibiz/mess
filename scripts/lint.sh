#!/usr/bin/env bash
set -eo pipefail

abs() { echo "$(cd "$(dirname "$1")" && pwd)/$(basename "$1")"; }
SCRIPT_DIR="$(dirname "$(abs "$0")")"
ROOT_DIR="$(dirname "$SCRIPT_DIR")"

export ASPNETCORE_ENVIRONMENT=Production
export DOTNET_ENVIRONMENT=Production
export ORCHARD_APP_DATA="$ROOT_DIR/App_Data"

export NODE_OPTIONS="--no-warnings"
export NODE_ENV="production"

printf "[Mess] Checking with 'csharpier'...\n"
dotnet csharpier . --check
printf "\n"

printf "[Mess] Checking with 'prettier'...\n"
yarn lint:prettier
printf "\n"

printf "[Mess] Linting with 'yarn'...\n"
yarn lint:workspaces
printf "\n"

printf "[Mess] Building with 'dotnet'...\n"
dotnet \
  build \
  --property "PublishDir=$ROOT_DIR/artifacts" \
  --property:TreatWarningsAsErrors=true \
  --configuration Release
printf "\n"
