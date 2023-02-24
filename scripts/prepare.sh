#!/usr/bin/env bash
set -eo pipefail

abs() { echo "$(cd "$(dirname "$1")" && pwd)/$(basename "$1")"; }
SCRIPT_DIR="$(dirname "$(abs "$0")")"
ROOT_DIR="$(dirname "$SCRIPT_DIR")"

export NODE_OPTIONS="--no-warnings"

printf "[Mess] Printing version information...\n"
printf "bash version:\n%s\n\n" "$(bash --version)"
printf "docker version:\n%s\n\n" "$(docker --version)"
printf "docker-compose version:\n%s\n\n" "$(docker-compose --version)"
printf "yarn version:\n%s\n\n" "$(yarn --version)"
printf "dotnet version:\n%s\n\n" "$(dotnet --version)"
printf "\n"

printf "[Mess] Installing dependencies with 'yarn'...\n"
yarn install
printf "\n"

printf "[Mess] Installing dependencies with 'dotnet'...\n"
dotnet restore
dotnet tool restore
printf "\n"

printf "[Mess] Creating containers with 'docker-compose'...\n"
if [ "$CI" ]; then
  docker-compose up --no-start --env-file "$ROOT_DIR/ci.env"
else
  docker-compose up --no-start
fi
printf "\n"

if [ ! "$CI" ]; then
  printf "[Mess] Setting up secrets with 'dotnet'...\n"
  dotnet user-secrets set --project "$ROOT_DIR/src/Mess.Web/Mess.Web.csproj" <"secrets.json"
  if [ ! -d "$ROOT_DIR/secrets" ]; then
    mkdir "$ROOT_DIR/secrets"
  fi
  cp "$ROOT_DIR/.env" "$ROOT_DIR/secrets"
  cp "$ROOT_DIR/secrets.json" "$ROOT_DIR/secrets"
  cp "$ROOT_DIR/secrets.sh" "$ROOT_DIR/secrets"
  # TODO: with glob
  if [ ! -d "$ROOT_DIR/secrets/test/Mess.Modules/Mess.MeasurementDevice.Test/assets" ]; then
    mkdir -p "$ROOT_DIR/secrets/test/Mess.Modules/Mess.MeasurementDevice.Test"
  fi
  cp -r \
    "$ROOT_DIR/test/Mess.Modules/Mess.MeasurementDevice.Test/assets" \
    "$ROOT_DIR/secrets/test/Mess.Modules/Mess.MeasurementDevice.Test"
  printf "\n"
fi

if [ ! "$CI" ] && [ ! -f "$ROOT_DIR/.husky/_/husky.sh" ]; then
  printf "[Mess] Setting up git hooks with 'husky'...\n"
  yarn husky install
  printf "\n"
fi
