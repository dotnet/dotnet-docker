#!/usr/bin/env pwsh

# Removes the 'needs-backport' label from the given pull requests in
# dotnet/dotnet-docker. Run this after successfully backporting changes to clear
# the label from PRs that have been handled.

param(
    [Parameter(Mandatory = $true)]
    [int[]] $PRs
)

$label = "needs-backport"

foreach ($pr in $PRs) {
    Write-Host "Removing '$label' from #$pr..."
    gh pr edit $pr --repo dotnet/dotnet-docker --remove-label $label
    if ($LASTEXITCODE -ne 0) {
        Write-Warning "Failed to remove '$label' from #$pr."
    }
}
