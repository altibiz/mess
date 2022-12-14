#!/usr/bin/env sh

# shellcheck disable=SC2068

printf "[Mess] Cleaning artifacts...\n"
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
  $@
printf "\n"
