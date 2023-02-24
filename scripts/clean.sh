#!/usr/bin/env bash
set -eo pipefail

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
    \
    -e '!.env' \
    -e '!secrets.json' \
    -e '!secrets.sh' \
    -e '!secrets/' \
    -e '!secrets/**' \
    -e '!**/*Test/assets' \
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
