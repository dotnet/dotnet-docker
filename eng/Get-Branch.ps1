#!/usr/bin/env pwsh

$repoRoot = (Get-Item "$PSScriptRoot").Parent.FullName
$manifestJson = Get-Content ${repoRoot}/manifest.json | ConvertFrom-Json
if ($manifestJson.Repos[0].Name.Contains("nightly")) {
    return "nightly"
}
else {
    return "main"
}
