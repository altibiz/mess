#!/usr/bin/env sh

abs() { echo "$(cd "$(dirname "$1")" && pwd)/$(basename "$1")"; }
SCRIPT_DIR="$(dirname "$(abs "$0")")"
ROOT_DIR="$(dirname "$SCRIPT_DIR")"

TYPE="$1"
NAME="$2"
if ! echo "$TYPE" | grep -Eq "(module|theme)"; then
  printf "[Mess] First argument (type) must be either 'module' or 'theme'\n"
  exit 1
fi

if ! echo "$NAME" | grep -Eq "[A-Za-z]+"; then
  printf "[Mess] Second argument (name) must consist only of letters\n"
  exit 1
fi
LOWERCASE_NAME="$(
  echo "$NAME" |
    sed 's/\(.\)\([A-Z]\)/\1-\2/g' |
    tr '[:upper:]' '[:lower:]'
)"

if [ "$TYPE" = "module" ]; then
  BASE_DIR="$ROOT_DIR/src/Mess.Modules/Mess.$NAME"
  printf "[Mess] Creating new module in '%s'\n" "$BASE_DIR"
else
  BASE_DIR="$ROOT_DIR/src/Mess.Themes/Mess.$NAME"
  printf "[Mess] Creating new theme in '%s'\n" "$BASE_DIR"
fi
if [ -d "$BASE_DIR" ]; then
  printf "[Mess] Directory '%s' already exists\n" "$BASE_DIR"
  exit 1
fi
mkdir "$BASE_DIR"
printf "\n"

CSPROJ="$BASE_DIR/Mess.$NAME.csproj"
PLACEHOLDER="$BASE_DIR/Mess.$NAME.cs"
NAMESPACE="Mess.$NAME"
printf "[Mess] Adding new C# project '%s'\n" "$CSPROJ"
cat <<END >"$CSPROJ"
<Project Sdk="Microsoft.NET.Sdk.Razor">

</Project>
END
cat <<END >"$PLACEHOLDER"
namespace $NAMESPACE;
END
dotnet sln add "$CSPROJ"
printf "\n"

ASSETS="$BASE_DIR/Assets"
PACKAGE="$ASSETS/package.json"
PACKAGE_NAME="@mess/$LOWERCASE_NAME"
printf "[Mess] Adding new NPM package '%s' in '%s'\n" "$PACKAGE_NAME" "$ASSETS"
cp -r "$ROOT_DIR/src/Mess.Assets/skeleton" "$ASSETS"
sed -i'' "s/name-of-module-or-theme/$LOWERCASE_NAME/" "$PACKAGE"
printf "\n"
