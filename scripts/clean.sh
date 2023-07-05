#!/usr/bin/env bash
set -eo pipefail

abs() { echo "$(cd "$(dirname "$1")" && pwd)/$(basename "$1")"; }
SCRIPT_DIR="$(dirname "$(abs "$0")")"
ROOT_DIR="$(dirname "$SCRIPT_DIR")"

# shellcheck disable=SC2068
git_clean() {
  git clean -Xd \
    \
    -e '!.husky/' \
    -e '!.husky/_/' \
    -e '!.husky/_/*' \
    -e '!.yarn/**' \
    -e '!.pnp.cjs' \
    -e '!.pnp.loader.mjs' \
    -e '!.vs/' \
    -e '!.vs/**' \
    -e '!**/*.csproj.user' \
    -e '!.vscode/**' \
    -e '!.env' \
    \
    -e '!secrets.json' \
    -e '!secrets.sh' \
    -e '!secrets/' \
    -e '!secrets/**' \
    \
    -e '!App_Data/' \
    $@
}

git_clean -n
printf "[Mess] Do you want to clean these artifacts? (y/n) "
read -r yn
printf "\n"
case $yn in
[Yy])
  printf "[Mess] Cleaning artifacts...\n"
  git_clean -f
  printf "\n"
  ;;
esac

if [ -d "$ROOT_DIR/App_data" ]; then
  printf "[Mess] Do you want to clean Orchard Core App Data? (y/n) "
  read -r yn
  printf "\n"
  case $yn in
  [Yy])
    printf "[Mess] Cleaning Orchard Core App Data...\n"
    rm -rf "$ROOT_DIR/App_Data"
    printf "\n"
    ;;
  esac
fi

printf "[Mess] Do you want to clean docker containers and volumes? (y/n) "
read -r yn
printf "\n"
case $yn in
[Yy])
  printf "[Mess] Cleaning docker containers and volumes...\n"
  docker-compose down -v
  printf "\n"
  ;;
esac
