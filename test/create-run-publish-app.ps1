[cmdletbinding()]
param(
   [string]$AppDirectory
)

Set-StrictMode -Version Latest
$ErrorActionPreference="Stop"

cd $AppDirectory
dotnet new
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

dotnet publish -o publish
if (-NOT $?) {
    throw  "Failed to publish app"
}
