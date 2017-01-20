[cmdletbinding()]
param(
   [string]$AppDirectory,
   [string]$SdkTag
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

cd $AppDirectory
if ($SdkTag -eq "1.1-sdk-msbuild-nanoserver") {
    dotnet new -t Console1.1
}
else {
    dotnet new
}

if (-NOT $?) {
    throw  "Failed to create project"
}

dotnet restore
if (-NOT $?) {
    throw  "Failed to restore packages"
}

dotnet run
if (-NOT $?) {
    throw  "Failed to run app"
}

dotnet publish -o publish/framework-dependent
if (-NOT $?) {
    throw  "Failed to publish app"
}
