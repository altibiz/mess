#!/usr/bin/env sh

abs() { echo "$(cd "$(dirname "$1")" && pwd)/$(basename "$1")"; }
SCRIPT_DIR="$(dirname "$(abs "$0")")"
ROOT_DIR="$(dirname "$SCRIPT_DIR")"

export ASPNETCORE_ENVIRONMENT=Development
export DOTNET_ENVIRONMENT=Development
export ORCHARD_APP_DATA="$ROOT_DIR/App_Data"

PROJECT="$1"
if ! echo "$PROJECT" | grep -Eq '^[A-Za-z\.]+$'; then
  printf "[Mess] The project name has to consist only of letters and dots\n"
  exit 1
fi

NAME="$2"
if ! echo "$NAME" | grep -Eq '^[A-Za-z]+$'; then
  printf "[Mess] The migration name has to consist only of letters\n"
  exit 1
fi

migrate_project() {
  printf "[Mess] Migrating '%s'\n" "$1"
  if [ ! -d "$ROOT_DIR/src/Mess.Modules/$1/Timeseries/Migrations" ]; then
    mkdir -p "$ROOT_DIR/src/Mess.Modules/$1/Timeseries/Migrations"
  fi
  dotnet ef \
    --startup-project "$ROOT_DIR/src/Mess.Web/Mess.Web.csproj" \
    --project "$ROOT_DIR/src/Mess.Modules/$1/$1.csproj" \
    migrations add \
    --output-dir "Timeseries/Migrations" \
    --namespace "$1.Timeseries.Migrations" \
    "$NAME" \
    ;
  dotnet csharpier \
    "$ROOT_DIR/src/Mess.Modules/$1/Timeseries/Migrations" \
    ;
  printf "\n"
}

migrate_project "$PROJECT"
