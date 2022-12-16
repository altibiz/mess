#!/usr/bin/env sh

# shellcheck disable=SC1090

abs() { echo "$(cd "$(dirname "$1")" && pwd)/$(basename "$1")"; }
SCRIPT_DIR="$(dirname "$(abs "$0")")"
ROOT_DIR="$(dirname "$SCRIPT_DIR")"
SECRETS="$ROOT_DIR/secrets.sh"
WORKING_DIR=$(pwd)

if [ "$1" ]; then
  PUBLISH_DIR="$1"
else
  PUBLISH_DIR="$ROOT_DIR/artifacts"
fi

export ASPNETCORE_ENVIRONMENT=Production
export DOTNET_ENVIRONMENT=Production
export ORCHARD_APP_DATA="$ROOT_DIR/App_Data"
export ORCHARD_VERSION="1.5.0"

export NODE_OPTIONS="--no-warnings"
export NODE_ENV="production"
export MESS_CLIENT_OUTPUT_DIRS="$ROOT_DIR/src/Mess.Client/dist"

stop() {
  printf "\n"
  printf "[Mess] Giving everything 1 second to stop properly...\n"
  sleep 1s
  printf "\n"
}
trap 'stop' INT

printf "[Mess] Building with 'yarn'..."
yarn build
printf "\n"

printf "[Mess] Publishing with 'dotnet'..."
dotnet \
  publish \
  --output "$PUBLISH_DIR" \
  --property:consoleLoggerParameters=ErrorsOnly \
  --property:IsWebConfigTransformDisabled=true \
  --configuration Release

printf "[Mess] Running 'docker-compose up' and 'dotnet'...\n"
printf "
docker-compose up; \

. '%s'; \
cd '%s' || exit; \
dotnet 'Mess.Web.dll'; \
cd '%s'; \

" "$SECRETS" "$PUBLISH_DIR" "$WORKING_DIR" |
  xargs -P2 -IR /usr/bin/env sh -c R
printf "\n"
