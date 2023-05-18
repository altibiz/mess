#!/usr/bin/env bash
set -eo pipefail

abs() { echo "$(cd "$(dirname "$1")" && pwd)/$(basename "$1")"; }
SCRIPT_DIR="$(dirname "$(abs "$0")")"
ROOT_DIR="$(dirname "$SCRIPT_DIR")"

TYPE="$1"
NAME="$2"
if ! echo "$TYPE" | grep -Eq "^(module|theme)$"; then
  printf "[Mess] First argument (type) must be either 'module' or 'theme'\n"
  exit 1
fi

if ! echo "$NAME" | grep -Eq "^[A-Za-z]+$"; then
  printf "[Mess] Second argument (name) must consist only of letters\n"
  exit 1
fi
LOWERCASE_NAME="$(
  echo "$NAME" |
    sed 's/\(.\)\([A-Z]\)/\1-\2/g' |
    tr '[:upper:]' '[:lower:]'
)"
LONG_NAME="${NAME//\([a-z]\)\([A-Z]\)/\1 \2/g}"

if ! echo "$DESCRIPTION" | grep -Eq "^[0-9A-Za-z \.,']+$"; then
  printf "[Mess] Third argument (description) must consist only of letters, numbers, spaces and punctuation\n"
  exit 1
fi

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

  <ItemGroup>
    <ProjectReference Include="../../Mess.Abstractions/Mess.System/Mess.System.csproj" />
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
  Tags = new[] { ManifestConstants.MessTag, ManifestConstants.OzdsTag }
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
TSCONFIG="$ASSETS/tsconfig.json"
PACKAGE_NAME="@mess/$LOWERCASE_NAME"
printf "[Mess] Adding new NPM package '%s' in '%s'\n" "$PACKAGE_NAME" "$ASSETS"
cp -r "$ROOT_DIR/src/Mess.Assets/skeleton" "$ASSETS"
sed -i'' "s/\.\.\/tsconfig.json/../../../Mess.Assets/tsconfig.json/" "$TSCONFIG"
sed -i'' "s/skeleton/$LOWERCASE_NAME/" "$PACKAGE"
printf "\n"

if [ "$TYPE" == "module" ]; then
  ABSTRACTIONS_NAMESPACE="Mess.$NAME.Abstractions"
  ABSTRACTIONS_BASE_DIR="$ROOT_DIR/src/Mess.Abstractions/$ABSTRACTIONS_NAMESPACE"
  ABSTRACTIONS_CSPROJ="$ABSTRACTIONS_BASE_DIR/$ABSTRACTIONS_NAMESPACE.csproj"
  ABSTRACTIONS_PLACEHOLDER="$ABSTRACTIONS_BASE_DIR/$ABSTRACTIONS_NAMESPACE.cs"
  cat <<END >"$MANIFEST"
using OrchardCore.Modules.Manifest;
using ManifestConstants = Mess.OrchardCore.ManifestConstants;

[assembly: Module(
  Name = "$LONG_NAME",
  Author = ManifestConstants.Author,
  Website = ManifestConstants.Website,
  Version = ManifestConstants.Version,
  Tags = new[] { ManifestConstants.MessTag, ManifestConstants.OzdsTag }
)]

[assembly: Feature(
  Id = "$NAMESPACE",
  Name = "$LONG_NAME",
  Description = "$DESCRIPTION",
  Category = ManifestConstants.Category
)]
END
  cat <<END >"$CSPROJ"
<Project Sdk="Microsoft.NET.Sdk.Razor">

  <ItemGroup>
    <ProjectReference Include="../../Mess.Abstractions/Mess.System/Mess.System.csproj" />
    <ProjectReference Include="../../Mess.Abstractions/$ABSTRACTIONS_NAMESPACE/$ABSTRACTIONS_NAMESPACE.csproj" />
  </ItemGroup>

</Project>
END
  cat <<END >"$ABSTRACTIONS_CSPROJ"
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <OutputType>Library</OutputType>
  </PropertyGroup>

</Project>
END
  cat <<END >"$ABSTRACTIONS_PLACEHOLDER"
namespace $ABSTRACTIONS_NAMESPACE;
END

  TEST_ABSTRACTIONS_NAMESPACE="Mess.$NAME.Test.Abstractions"
  TEST_ABSTRACTIONS_BASE_DIR="$ROOT_DIR/test/Mess.Abstractions/$TEST_ABSTRACTIONS_NAMESPACE"
  TEST_ABSTRACTIONS_CSPROJ="$TEST_ABSTRACTIONS_BASE_DIR/$TEST_ABSTRACTIONS_NAMESPACE.csproj"
  TEST_ABSTRACTIONS_PLACEHOLDER="$TEST_ABSTRACTIONS_BASE_DIR/$TEST_ABSTRACTIONS_NAMESPACE.cs"
  cat <<END >"$TEST_ABSTRACTIONS_CSPROJ"
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <OutputType>Library</OutputType>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="../../../src/Mess.Abstractions/Mess.System/Mess.System.csproj" />
    <ProjectReference Include="../../Mess.Abstractions/Mess.Xunit/Mess.Xunit.csproj" />
    <ProjectReference Include="../../../src/Mess.Abstractions/$ABSTRACTIONS_NAMESPACE/$ABSTRACTIONS_NAMESPACE.csproj" />
  </ItemGroup>

</Project>
END
  cat <<END >"$TEST_ABSTRACTIONS_PLACEHOLDER"
namespace $TEST_ABSTRACTIONS_NAMESPACE;
END
  cat <<END >"$TEST_ABSTRACTIONS_PLACEHOLDER"
namespace $TEST_ABSTRACTIONS_NAMESPACE;
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
    <ProjectReference Include="../../Mess.Abstractions/Mess.Xunit/Mess.Xunit.csproj" />
    <ProjectReference Include="../../../src/Mess.Abstractions/$ABSTRACTIONS_NAMESPACE/$ABSTRACTIONS_NAMESPACE.csproj" />
    <ProjectReference Include="../../../src/Mess.Modules/$NAMESPACE/$NAMESPACE.csproj" />
    <ProjectReference Include="../../Mess.Abstractions/$TEST_ABSTRACTIONS_NAMESPACE/$TEST_ABSTRACTIONS_NAMESPACE.csproj" />
  </ItemGroup>

</Project>
END
  cat <<END >"$TEST_PLACEHOLDER"
namespace $TEST_NAMESPACE;
END
  cat <<END >"$TEST_STARTUP"
using Xunit.DependencyInjection;
using Xunit.DependencyInjection.Logging;
using Mess.Xunit.Extensions.Microsoft;

namespace $TEST_NAMESPACE;

public class Startup
{
  public IHostBuilder CreateHostBuilder()
  {
    return Host.CreateDefaultBuilder();
  }

  public void ConfigureHost(IHostBuilder hostBuilder)
  {
    hostBuilder.ConfigureLogging(builder => builder.ClearProviders());
  }

  public void ConfigureServices(
    IServiceCollection services,
    HostBuilderContext hostBuilderContext
  )
  {
    services.RegisterTestTenants();
  }

  public void Configure(IServiceProvider services)
  {
    services
      .GetRequiredService<ILoggerFactory>()
      .AddProvider(
        new XunitTestOutputLoggerProvider(
          services.GetRequiredService<ITestOutputHelperAccessor>(),
          (_, level) => level is >= LogLevel.Debug and < LogLevel.None
        )
      );
  }
}
END
fi

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
