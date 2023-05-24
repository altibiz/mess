#!/usr/bin/env bash
set -eo pipefail

abs() { echo "$(cd "$(dirname "$1")" && pwd)/$(basename "$1")"; }
SCRIPT_DIR="$(dirname "$(abs "$0")")"
ROOT_DIR="$(dirname "$SCRIPT_DIR")"

TYPE="$1"
if ! echo "$TYPE" | grep -Eq "^(module|theme)$"; then
  printf "[Mess] First argument (type) must be either 'module' or 'theme'\n"
  printf "[Mess] Given first argument: '%s'" "$TYPE"
  exit 1
fi

NAME="$2"
if ! echo "$NAME" | grep -Eq "^[A-Za-z]+$"; then
  printf "[Mess] Second argument (name) must consist only of letters\n"
  printf "[Mess] Given second argument: '%s'" "$NAME"
  exit 1
fi
LOWERCASE_NAME="$(
  echo "$NAME" |
    sed 's/\(.\)\([A-Z]\)/\1-\2/g' |
    tr '[:upper:]' '[:lower:]'
)"
LONG_NAME="${NAME//\([a-z]\)\([A-Z]\)/\1 \2/g}"

DESCRIPTION="$3"
if ! echo "$DESCRIPTION" | grep -Eq "^[0-9A-Za-z \.,'\"]+$"; then
  printf "[Mess] Third argument (description) must consist only of letters, numbers, spaces and punctuation\n"
  printf "[Mess] Given third argument: '%s'" "$DESCRIPTION"
  exit 1
fi

if [ "$TYPE" = "module" ]; then
  BASE_DIR="$ROOT_DIR/src/Mess.Modules/Mess.$NAME"
  ABSTRACTIONS_BASE_DIR="$ROOT_DIR/src/Mess.Abstractions/Mess.$NAME.Abstractions"
  TEST_BASE_DIR="$ROOT_DIR/test/Mess.Modules/Mess.$NAME.Test"
  TEST_ABSTRACTIONS_BASE_DIR="$ROOT_DIR/test/Mess.Abstractions/Mess.$NAME.Test.Abstractions"
  printf "[Mess] Creating new module in '%s'\n" "$BASE_DIR"
  printf "       with abstractions in '%s'\n" "$ABSTRACTIONS_BASE_DIR"
  printf "       with tests in '%s'\n" "$TEST_BASE_DIR"
  printf "       with test abstractions in '%s'\n" "$TEST_ABSTRACTIONS_BASE_DIR"
  printf "\n"
else
  BASE_DIR="$ROOT_DIR/src/Mess.Themes/Mess.$NAME"
  printf "[Mess] Creating new theme in '%s'\n" "$BASE_DIR"
fi
if [ -d "$BASE_DIR" ]; then
  printf "[Mess] Directory '%s' already exists\n" "$BASE_DIR"
  exit 1
fi
if [ "$TYPE" = "module" ]; then
  if [ -d "$ABSTRACTIONS_BASE_DIR" ]; then
    printf "[Mess] Directory '%s' already exists\n" "$ABSTRACTIONS_BASE_DIR"
    exit 1
  fi
  if [ -d "$TEST_BASE_DIR" ]; then
    printf "[Mess] Directory '%s' already exists\n" "$TEST_BASE_DIR"
    exit 1
  fi
  if [ -d "$TEST_ABSTRACTIONS_BASE_DIR" ]; then
    printf "[Mess] Directory '%s' already exists\n" "$TEST_ABSTRACTIONS_BASE_DIR"
    exit 1
  fi
fi
mkdir "$BASE_DIR"
if [ "$TYPE" = "module" ]; then
  mkdir "$ABSTRACTIONS_BASE_DIR"
  mkdir "$TEST_BASE_DIR"
  mkdir "$TEST_ABSTRACTIONS_BASE_DIR"
fi
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

<ItemGroup>
  <ProjectReference Include="../../../src/Mess.Abstractions/Mess.System/Mess.System.csproj" />
  <ProjectReference Include="../../../src/Mess.Abstractions/Mess.Tenants/Mess.Tenants.csproj" />
  <ProjectReference Include="../../../src/Mess.Abstractions/Mess.OrchardCore/Mess.OrchardCore.csproj" />
</ItemGroup>

</Project>
END
cat <<END >"$PLACEHOLDER"
namespace $NAMESPACE;
END
cat <<END >"$MANIFEST"
using OrchardCore.DisplayManagement.Manifest;
using Mess.OrchardCore;

[assembly: Theme(
  Name = "The $LONG_NAME Theme",
  Author = ManifestConstants.Author,
  Website = ManifestConstants.Website,
  Version = ManifestConstants.Version,
  Description = "$DESCRIPTION",
  Category = ManifestConstants.Category,
  Tags = new string[] { ManifestConstants.MessTag }
)]
END
cat <<END >"$STARTUP"
using Microsoft.AspNetCore.Builder;
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

  public override void Configure(
    IApplicationBuilder app,
    Microsoft.AspNetCore.Routing.IEndpointRouteBuilder routes,
    IServiceProvider serviceProvider
  )
  {
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
dotnet sln add "$CSPROJ"
printf "\n"

ASSETS="$BASE_DIR/Assets"
PACKAGE="$ASSETS/package.json"
TSCONFIG="$ASSETS/tsconfig.json"
if [ "$TYPE" == "module" ]; then
  PACKAGE_NAME="@mess/$LOWERCASE_NAME-module-assets"
else
  PACKAGE_NAME="@mess/$LOWERCASE_NAME-theme-assets"
fi
printf "[Mess] Adding new NPM package '%s' in '%s'\n" "$PACKAGE_NAME" "$ASSETS"
cp -r "$ROOT_DIR/src/Mess.Assets/skeleton" "$ASSETS"
sed -i'' 's/\.\.\/tsconfig.json/..\/..\/..\/Mess.Assets\/tsconfig.json/' "$TSCONFIG"
sed -i'' "s/@mess\/assets-skeleton/$PACKAGE_NAME/" "$PACKAGE"
printf "\n"

if [ "$TYPE" == "module" ]; then
  ABSTRACTIONS_NAMESPACE="Mess.$NAME.Abstractions"
  ABSTRACTIONS_CSPROJ="$ABSTRACTIONS_BASE_DIR/$ABSTRACTIONS_NAMESPACE.csproj"
  ABSTRACTIONS_PLACEHOLDER="$ABSTRACTIONS_BASE_DIR/$ABSTRACTIONS_NAMESPACE.cs"
  cat <<END >"$ABSTRACTIONS_CSPROJ"
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <OutputType>Library</OutputType>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="../../../src/Mess.Abstractions/Mess.System/Mess.System.csproj" />
    <ProjectReference Include="../../../src/Mess.Abstractions/Mess.Tenants/Mess.Tenants.csproj" />
    <ProjectReference Include="../../../src/Mess.Abstractions/Mess.OrchardCore/Mess.OrchardCore.csproj" />
  </ItemGroup>

</Project>
END
  cat <<END >"$ABSTRACTIONS_PLACEHOLDER"
namespace $ABSTRACTIONS_NAMESPACE;
END

  TEST_NAMESPACE="Mess.$NAME.Test"
  TEST_BASE_DIR="$ROOT_DIR/test/Mess.Modules/$TEST_NAMESPACE"
  TEST_CSPROJ="$TEST_BASE_DIR/$TEST_NAMESPACE.csproj"
  TEST_PLACEHOLDER="$TEST_BASE_DIR/$TEST_NAMESPACE.cs"
  TEST_STARTUP="$TEST_BASE_DIR/Startup.cs"
  cat <<END >"$TEST_CSPROJ"
<Project Sdk="Microsoft.NET.Sdk.Web">

  <ItemGroup>
    <ProjectReference Include="../../../src/Mess.Abstractions/Mess.System/Mess.System.csproj" />
    <ProjectReference Include="../../../src/Mess.Abstractions/Mess.Tenants/Mess.Tenants.csproj" />
    <ProjectReference Include="../../../src/Mess.Abstractions/Mess.OrchardCore/Mess.OrchardCore.csproj" />
    <ProjectReference Include="../../../src/Mess.Abstractions/$ABSTRACTIONS_NAMESPACE/$ABSTRACTIONS_NAMESPACE.csproj" />
    <ProjectReference Include="../../../src/Mess.Modules/$NAMESPACE/$NAMESPACE.csproj" />
    <ProjectReference Include="../../../test/Mess.Abstractions/Mess.Test/Mess.Test.csproj" />
    <ProjectReference Include="../../../test/Mess.Abstractions/Mess.OrchardCore.Test/Mess.OrchardCore.Test.csproj" />
    <ProjectReference Include="../../../test/Mess.Abstractions/$TEST_ABSTRACTIONS_NAMESPACE/$TEST_ABSTRACTIONS_NAMESPACE.csproj" />
  </ItemGroup>

</Project>
END
  cat <<END >"$TEST_PLACEHOLDER"
namespace $TEST_NAMESPACE;
END
  cat <<END >"$TEST_STARTUP"
namespace $TEST_NAMESPACE;

public class Startup : Mess.OrchardCore.Test.Startup
{
  public override void ConfigureServices(
    IServiceCollection services,
    HostBuilderContext hostBuilderContext
  )
  {
    base.ConfigureServices(services, hostBuilderContext);
  }
}
END

  TEST_ABSTRACTIONS_NAMESPACE="Mess.$NAME.Test.Abstractions"
  TEST_ABSTRACTIONS_CSPROJ="$TEST_ABSTRACTIONS_BASE_DIR/$TEST_ABSTRACTIONS_NAMESPACE.csproj"
  TEST_ABSTRACTIONS_PLACEHOLDER="$TEST_ABSTRACTIONS_BASE_DIR/$TEST_ABSTRACTIONS_NAMESPACE.cs"
  cat <<END >"$TEST_ABSTRACTIONS_CSPROJ"
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <OutputType>Library</OutputType>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="../../../src/Mess.Abstractions/Mess.System/Mess.System.csproj" />
    <ProjectReference Include="../../../src/Mess.Abstractions/Mess.Tenants/Mess.Tenants.csproj" />
    <ProjectReference Include="../../../src/Mess.Abstractions/Mess.OrchardCore/Mess.OrchardCore.csproj" />
    <ProjectReference Include="../../../src/Mess.Abstractions/$ABSTRACTIONS_NAMESPACE/$ABSTRACTIONS_NAMESPACE.csproj" />
    <ProjectReference Include="../../../test/Mess.Abstractions/Mess.Test/Mess.Test.csproj" />
    <ProjectReference Include="../../../test/Mess.Abstractions/Mess.OrchardCore.Test/Mess.OrchardCore.Test.csproj" />
  </ItemGroup>

</Project>
END
  cat <<END >"$TEST_ABSTRACTIONS_PLACEHOLDER"
namespace $TEST_ABSTRACTIONS_NAMESPACE;
END

  cat <<END >"$MANIFEST"
using OrchardCore.Modules.Manifest;
using ManifestConstants = Mess.OrchardCore.ManifestConstants;

[assembly: Module(
  Name = "$LONG_NAME",
  Author = ManifestConstants.Author,
  Website = ManifestConstants.Website,
  Version = ManifestConstants.Version,
  Tags = new string[] { ManifestConstants.MessTag }
)]

[assembly: Feature(
  Id = "$NAMESPACE",
  Name = "$LONG_NAME",
  Description = "$DESCRIPTION",
  Category = ManifestConstants.Category,
  Dependencies = new string[] { }
)]
END
  cat <<END >"$CSPROJ"
<Project Sdk="Microsoft.NET.Sdk.Razor">

  <ItemGroup>
    <ProjectReference Include="../../../src/Mess.Abstractions/Mess.System/Mess.System.csproj" />
    <ProjectReference Include="../../../src/Mess.Abstractions/Mess.Tenants/Mess.Tenants.csproj" />
    <ProjectReference Include="../../../src/Mess.Abstractions/Mess.OrchardCore/Mess.OrchardCore.csproj" />
    <ProjectReference Include="../../../src/Mess.Abstractions/$ABSTRACTIONS_NAMESPACE/$ABSTRACTIONS_NAMESPACE.csproj" />
  </ItemGroup>

  <ItemGroup>
    <InternalsVisibleTo Include="../../../test/Mess.Modules/$TEST_NAMESPACE/$TEST_NAMESPACE.csproj" />
  </ItemGroup>

</Project>
END
fi

printf "[Mess] Do you want to run 'format.sh'? (y/n) "
read -r yn
printf "\n"
case $yn in
[Yy])
  printf "[Mess] Running 'format.sh'...\n"
  "$SCRIPT_DIR/format.sh"
  printf "\n"
  ;;
esac

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
