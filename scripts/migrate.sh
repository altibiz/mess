#!/usr/bin/env sh

abs() { echo "$(cd "$(dirname "$1")" && pwd)/$(basename "$1")"; }
SCRIPT_DIR="$(dirname "$(abs "$0")")"
ROOT_DIR="$(dirname "$SCRIPT_DIR")"

export ASPNETCORE_ENVIRONMENT=Development
export DOTNET_ENVIRONMENT=Development
export ORCHARD_APP_DATA="$ROOT_DIR/App_Data"

if [ ! "$1" ]; then
  printf "Please name your migration\n"
  exit 1
fi

dotnet ef \
  --startup-project "$ROOT_DIR/src/Mess.Web/Mess.Web.csproj" \
  --project "$ROOT_DIR/src/Mess.Timeseries/Mess.Timeseries.csproj" \
  migrations add \
  --output-dir "Migrations/Timescale" \
  --namespace "Mess.Migrations.Timescale" \
  "$1"

dotnet csharpier "$ROOT_DIR/src/Mess.Timeseries/Migrations/Timescale"
