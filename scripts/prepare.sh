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
dotnet tool restore
dotnet restore
dotnet build
pwsh \
  "$ROOT_DIR"/test/Mess.System.Test/bin/Debug/**/playwright.ps1 \
  install --with-deps
printf "\n"

if [ ! "$CI" ]; then
  printf "[Mess] Creating containers with 'docker-compose'...\n"
  docker-compose up --no-start
  printf "\n"
fi

if [ ! "$CI" ]; then
  printf "[Mess] Setting up secrets with 'dotnet'...\n"

  if [ ! -f "$ROOT_DIR/secrets.json" ]; then
    printf "[Mess] File 'secrets.json' not found.\n"
    printf "[Mess] Please copy over secrets to the worktree.\n"
    printf "\n"
    exit 1
  fi

  if [ ! -f "$ROOT_DIR/secrets.sh" ]; then
    printf "[Mess] File 'secrets.sh' not found.\n"
    printf "[Mess] Please copy over secrets to the worktree.\n"
    printf "\n"
    exit 1
  fi

  dotnet user-secrets set --project "$ROOT_DIR/src/Mess.Web/Mess.Web.csproj" <"secrets.json"

  if [ ! -d "$ROOT_DIR/secrets" ]; then
    mkdir "$ROOT_DIR/secrets"
  fi
  cp "$ROOT_DIR/secrets.json" "$ROOT_DIR/secrets"
  cp "$ROOT_DIR/secrets.sh" "$ROOT_DIR/secrets"

  printf "\n"
fi

if [ ! "$CI" ] && [ ! -f "$ROOT_DIR/.husky/_/husky.sh" ]; then
  printf "[Mess] Setting up git hooks with 'husky'...\n"
  yarn husky install
  printf "\n"
fi
