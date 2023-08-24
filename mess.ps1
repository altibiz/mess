#!/usr/bin/env pwsh

$ROOT_DIR = [System.IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)

$DOCKER_URL = "https://docs.docker.com/engine/install/"

$DOTNET_VERSION = $(
  Get-Content -Path "$ROOT_DIR/global.json" | ConvertFrom-Json
).sdk.version.Split('.')[0]
$DOTNET_INSTALL_URL = "https://dot.net/v1/dotnet-install.ps1"
$DOTNET_DOWNLOAD_URL = "https://dotnet.microsoft.com/download"
$DOTNET_INSTALL_QUESTION = "Would you like to install version $DOTNET_VERSION using dotnet-install?"
$DOTNET_INSTALL_PROMPT = ".NET SDK is not installed. $DOTNET_INSTALL_QUESTION"
$DOTNET_VERSION_PROMPT = "Incorrect .NET SDK version. $DOTNET_INSTALL_QUESTION"
$DOTNET_INSTALLED = "Installed .NET SDK $DOTNET_VERSION."

$NODE_VERSION = Get-Content -Path "$ROOT_DIR/.nvmrc"
$NODE_URL = "https://nodejs.org/en/download"
$NODE_INSTALL_QUESTION = "Would you like to install version $NODE_VERSION using NVM?"
$NODE_INSTALL_PROMPT = "Node.js is not installed. $NODE_INSTALL_QUESTION"
$NODE_VERSION_PROMPT = "Incorrect Node.js version. $NODE_INSTALL_QUESTION"
$NODE_INSTALLED = "Installed Node.js $NODE_VERSION."

$YARN_URL="https://yarnpkg.com/getting-started/install"
$YARN_INSTALL_QUESTION = "Would you like to install yarn?"
$YARN_INSTALL_PROMPT = "Yarn is not installed. $YARN_INSTALL_QUESTION"
$YARN_INSTALLED = "Installed yarn."

function Install-Dependency(
  [String] $InstallPrompt,
  [String] $InstalledVersion = $null,
  [String] $InstallCommand,
  [String] $InstalledMessage,
  [String] $DownloadUrl
) {
  if ([String]::IsNullOrWhiteSpace(($InstalledVersion))) {
    Write-Host -NoNewline "$InstallPrompt (y/n): "
  } else {
    Write-Host -NoNewline "$InstallPrompt (current $InstalledVersion) (y/n): "
  }
  $Choice = Read-Host
  if ($Choice -eq "y") {
    try {
      Invoke-Expression "$InstallCommand" > $null 2>&1
    } catch {
      Write-Host "Failed to install. Exiting..."
      exit 1
    }
  } else {
    Write-Host "Opening download page..."
    Start-Process $DownloadUrl
    exit 1
  }

  Write-Host "$InstalledMessage"
  $env:Path = `
    [System.Environment]::GetEnvironmentVariable("Path","Machine") + ";" + `
    [System.Environment]::GetEnvironmentVariable("Path","User")
}

function Test-Installed(
  [String] $Command,
  [String] $InstallPrompt,
  [String] $InstallCommand,
  [String] $InstalledMessage,
  [String] $DownloadUrl
) {
  try {
    Invoke-Expression "$Command" >$null 2>&1
  } catch {
    Install-Dependency `
      -InstallPrompt "$InstallPrompt" `
      -InstallCommand "$InstallCommand" `
      -InstalledMessage "$InstalledMessage" `
      -DownloadUrl "$DownloadUrl"

    return $true
  }

  return $false
}

function Test-Version(
  [String] $Command,
  [String] $ExpectedVersion,
  [String] $InstallPrompt,
  [String] $InstallCommand,
  [String] $InstalledMessage,
  [String] $DownloadUrl
) {
  try {
    $InstalledVersion = Invoke-Expression "$Command" 2>&1
    if ($InstalledVersion -ne $ExpectedVersion) {
      throw
    }
  } catch {
    Install-Dependency `
      -InstallPrompt "$InstallPrompt" `
      -InstalledVersion $InstalledVersion `
      -InstallCommand "$InstallCommand" `
      -InstalledMessage "$InstalledMessage" `
      -DownloadUrl "$DownloadUrl" `

    return $true
  }

  return $false
}

function Get-Dotnet-Version {
  $version = Invoke-Expression "dotnet --version" 2>&1
  return ($version -split '\.')[0]
}

function Get-Node-Version {
  $version = Invoke-Expression "node --version" 2>&1
  # TODO: once loaders get stabilized...
  # return ($version -replace '^v(\d+)\..*', '$1')
  return ($version -replace '^v', '')
}

function Install-Dotnet {
  $InstallDir = if (Test-Path 'C:\Program Files\dotnet') {
    Write-Host "Installing .NET SDK $DOTNET_VERSION in 'C:\Program Files\dotnet'..."
    'C:\Program Files\dotnet'
  } else {
    Write-Host "Installing .NET SDK $DOTNET_VERSION in default location..."
    $null
  }

  $TempFile = [System.IO.Path]::GetTempFileName()
  $TempScript = [System.IO.Path]::ChangeExtension($TempFile, 'ps1')
  Invoke-WebRequest -Uri $DOTNET_INSTALL_URL -OutFile $TempScript

  $ExitCodeFile = [System.IO.Path]::GetTempFileName()
  $WrapperFile = [System.IO.Path]::GetTempFileName()
  $WrapperScript = [System.IO.Path]::ChangeExtension($WrapperFile, 'ps1')
  $WrapperContent = @"
`$exitCode = 0
try {
  Invoke-Expression "'$TempScript' -Version $DOTNET_VERSION -InstallDir '$InstallDir' -Verbose"
} catch {
  Write-Host 'An error occurred during installation!'
  `$exitCode = 1
}
`$exitCode | Out-File '$ExitCodeFile'
Read-Host 'Press any key to continue...'
"@
  Set-Content -Path $WrapperScript -Value $WrapperContent

  Start-Process powershell `
    -ArgumentList "-File", $WrapperScript `
    -Verb RunAs `
    -Wait
  $ExitCode = Get-Content -Path $ExitCodeFile

  Remove-Item -Path $ExitCodeFile
  Remove-Item -Path $TempScript
  Remove-Item -Path $WrapperScript

  if ($ExitCode -eq 1) {
    Write-Host "An error occurred during the .NET SDK installation."
    exit 1
  } else {
    Write-Host ".NET SDK installation was successful."
  }
}

function Install-Node {
  & nvm install $NODE_VERSION
  & nvm use $NODE_VERSION
}

function Install-Yarn {
  & npm install -g yarn
}

function Install-Yarn-Packages {
  $YarnOutput = $null
  try {
    $YarnOutput = Invoke-Expression "yarn install --json" 2>&1
  } catch {
    Write-Host "Failed to yarn install. Exiting..."
    exit 1
  }
  $YarnOutput -split "`n" | ForEach-Object {
    $Message = ConvertFrom-Json $_
    if ($Message.data.StartsWith("Done")) {
      Write-Host ("`nyarn installed dependencies" + $Message.data.Substring(4))
    } elseif ($Message.data.StartsWith("ESM support for PnP")) {
    } elseif ($Message.type -ne "info") {
      Write-Host $Message.data
    }
  }
  Write-Host -NoNewline "`n`n"
}

try {
  Invoke-Expression "docker --version" >$null 2>&1
} catch {
  Write-Host "Docker is not installed. Opening the download page..."
  Start-Process $DOCKER_URL
  exit 1
}

$InstalledDotnet = Test-Installed `
  -Command "dotnet --version" `
  -InstallPrompt "$DOTNET_INSTALL_PROMPT" `
  -InstallCommand "Install-Dotnet" `
  -DownloadUrl "$DOTNET_DOWNLOAD_URL" `
  -InstalledMessage "$DOTNET_INSTALLED"

if (-not $InstalledDotnet) {
  $InstalledNode = Test-Version `
    -Command "Get-Dotnet-Version" `
    -ExpectedVersion $DOTNET_VERSION `
    -InstallPrompt $DOTNET_VERSION_PROMPT `
    -InstallCommand "Install-Dotnet" `
    -InstalledMessage $DOTNET_INSTALLED `
    -DownloadUrl $DOTNET_DOWNLOAD_URL
}

$InstalledNode = Test-Installed `
  -Command "node --version" `
  -InstallPrompt $NODE_INSTALL_PROMPT `
  -InstallCommand "Install-Node" `
  -InstalledMessage $NODE_INSTALLED `
  -DownloadUrl $NODE_URL

if (-not $InstalledNode) {
  $InstalledNode = Test-Version `
    -Command "Get-Node-Version" `
    -ExpectedVersion $NODE_VERSION `
    -InstallPrompt $NODE_VERSION_PROMPT `
    -InstallCommand "Install-Node" `
    -InstalledMessage $NODE_INSTALLED `
    -DownloadUrl $NODE_URL
}

$InstalledYarn = Test-Installed `
  -Command "yarn --version" `
  -InstallPrompt "$YARN_INSTALL_PROMPT" `
  -InstallCommand "Install-Yarn" `
  -InstalledMessage "$YARN_INSTALLED" `
  -DownloadUrl "$YARN_URL"

if ($InstalledDotnet -or $InstalledNode -or $InstalledYarn) {
  Write-Host -NoNewline "`n`n"
}

Write-Output 'require("prettier")' | yarn node >$null 2>&1
if (!$?) { Install-Yarn-Packages }

$YarnArgs = $args -join "' '"
Invoke-Expression "yarn scripts start '$YarnArgs'"
