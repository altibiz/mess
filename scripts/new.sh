#!/usr/bin/env bash
set -eo pipefail

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

NAMESPACE="Mess.$NAME"
CSPROJ="$BASE_DIR/$NAMESPACE.csproj"
PLACEHOLDER="$BASE_DIR/$NAMESPACE.cs"
MANIFEST="$BASE_DIR/Manifest.cs"
STARTUP="$BASE_DIR/Startup.cs"
RESOURCES="$BASE_DIR/Resources.cs"
MIGRATIONS="$BASE_DIR/Migrations.cs"
printf "[Mess] Adding new C# project '%s'\n" "$CSPROJ"
cat <<END >"$CSPROJ"
<Project Sdk="Microsoft.NET.Sdk.Razor">

</Project>
END
cat <<END >"$PLACEHOLDER"
namespace $NAMESPACE;
END
cat <<END >"$MANIFEST"
using OrchardCore.Modules.Manifest;

[assembly: Module(
  Name = "$NAMESPACE",
  Author = "Altibiz",
  Website = "https://altibiz.com",
  Version = "0.0.1",
  Description = "$NAMESPACE",
  Category = "Content Management"
)]
END
cat <<END >"$STARTUP"
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.ResourceManagement;

namespace $NAMESPACE;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddDataMigration<Migrations>();
    services.AddTransient<
      IConfigureOptions<ResourceManagementOptions>,
      Resources
    >();
  }
}
END
cat <<END >"$RESOURCES"
using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace $NAMESPACE;

public class Resources : IConfigureOptions<ResourceManagementOptions>
{
  static Resources()
  {
    _manifest = new ResourceManifest();
  }

  public void Configure(ResourceManagementOptions options)
  {
    options.ResourceManifests.Add(_manifest);
  }

  private static ResourceManifest _manifest;
}
END
cat <<END >"$MIGRATIONS"
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;

namespace $NAMESPACE;

public class Migrations : DataMigration
{
  public int Create()
  {
    return 1;
  }

  public Migrations(
    IContentDefinitionManager contentDefinitionManager,
    IRecipeMigrator recipeMigrator
  )
  {
    _contentDefinitionManager = contentDefinitionManager;
    _recipeMigrator = recipeMigrator;
  }

  private readonly IContentDefinitionManager _contentDefinitionManager;
  private readonly IRecipeMigrator _recipeMigrator;
}
END
# TODO: better manifest control
dotnet sln add "$CSPROJ"
printf "\n"

ASSETS="$BASE_DIR/Assets"
PACKAGE="$ASSETS/package.json"
PACKAGE_NAME="@mess/$LOWERCASE_NAME"
printf "[Mess] Adding new NPM package '%s' in '%s'\n" "$PACKAGE_NAME" "$ASSETS"
cp -r "$ROOT_DIR/src/Mess.Assets/skeleton" "$ASSETS"
sed -i'' "s/name-of-module-or-theme/$LOWERCASE_NAME/" "$PACKAGE"
printf "\n"

printf "[Mess] Do you want to run 'prepare.sh'? (y/n) "
read -r yn
printf "\n"
case $yn in
  [Yy])
    printf "[Mess] Running 'prepare.sh'...\n"
    "$SCRIPT_DIR/prepare.sh"
    printf "\n"
    ;;
esac
