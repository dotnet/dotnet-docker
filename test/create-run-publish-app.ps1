[cmdletbinding()]
param(
   [string]$AppDirectory,
   [string]$SdkTag
)

Set-StrictMode -Version Latest
$ErrorActionPreference="Stop"

cd $AppDirectory
dotnet new
if (-NOT $?) {
    throw  "Failed to create project"
}

if ($SdkTag.StartsWith("1.1")) {
    (Get-Content project.json).replace("1.0.1", "1.1.0") | Set-Content project.json
}

dotnet restore
if (-NOT $?) {
    throw  "Failed to restore packages"
}

dotnet run
if (-NOT $?) {
    throw  "Failed to run app"
}

dotnet publish -o publish
if (-NOT $?) {
    throw  "Failed to publish app"
}
