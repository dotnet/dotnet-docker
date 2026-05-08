---
name: create-public-release-pr
description: Create a public PR to the dotnet-docker release branch with new .NET versions, and a follow-up PR from the release branch into main. Use when preparing the public release PR for a .NET containers servicing release.
user-invocable: true
disable-model-invocation: true
---

# Create Public Release PR

## Workflow

1. **Create a working branch** - create a new branch based off of the public release branch (e.g. `release-$ReleaseName`).
   - Run `pwsh ../shared/Get-ReleaseBranches.ps1` to find the latest release branch.
   - The most recently created branch corresponds to the current release.
2. **Collect stage container names** - read `stage-containers.txt` from the internal release branch get the stage container names for each release.
3. **Update Dockerfiles to new .NET versions** - for each .NET version to be released, run the update-dependencies tool from the root of the dotnet-docker repo:

   ```bash
   dotnet run --project ./eng/update-dependencies/update-dependencies.csproj -- from-staging-pipeline $StageContainerName --azdo-organization "https://dev.azure.com/dnceng" --azdo-project internal --source-branch "main"
   ```

   - Commit changes separately for each .NET version with this commit message format: `Update .NET X.0 to X.0.Y Runtime / X.0.ZZZ SDK`.
     For previews: `Update .NET X.0 to X.0 Preview N`.
4. **Confirm with the user** — Let the user review the changes.
5. **Submit the PR** — Push the branch to `origin` and submit a PR to `main`.
