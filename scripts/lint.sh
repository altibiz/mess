#!/usr/bin/env sh

abs() { echo "$(cd "$(dirname "$1")" && pwd)/$(basename "$1")"; }
SCRIPT_DIR="$(dirname "$(abs "$0")")"
ROOT_DIR="$(dirname "$SCRIPT_DIR")"

export ASPNETCORE_ENVIRONMENT=Production
export DOTNET_ENVIRONMENT=Production
export ORCHARD_APP_DATA="$ROOT_DIR/App_Data"
export ORCHARD_VERSION="1.5.0"

export NODE_OPTIONS="--no-warnings"
export NODE_ENV="production"

printf "[Mess] Linting with 'yarn'...\n"
yarn lint-children || exit 1
printf "\n"

printf "[Mess] Building with 'dotnet'...\n"
dotnet \
  build \
  --output "$ROOT_DIR/artifacts" \
  --property:TreatWarningsAsErrors=true \
  --configuration Release || exit 1
printf "\n"

printf "[Mess] Checking with 'csharpier'...\n"
dotnet csharpier . --check || exit 1
printf "\n"
