#!/usr/bin/env bash
set -euo pipefail

export NODE_OPTIONS="--no-warnings"

printf "[Mess] Formatting with 'yarn'...\n"
yarn format
printf "\n"

printf "[Mess] Formatting with 'dotnet'...\n"
dotnet csharpier .
printf "\n"
