---
name: merge-main-to-nightly
description: >-
  Create a PR to merge main into the nightly branch after a .NET containers
  release. Use only when specifically asked to "merge main to nightly".
---

# Workflow: Merge main to nightly

This should be done as soon as possible after a new .NET release (same day).

1. Determine the release name. Run `pwsh eng/Get-ReleaseBranches.ps1` to find the latest release branch. The most recently created branch corresponds to the current release.
2. Fetch changes from the dotnet/dotnet-docker `main` and `nightly` branches.
3. Create a new branch based off of the `nightly` branch, called `merge-main-to-nightly-$releaseName`.
4. Merge main into your working branch with `git merge $remote/main`. If there are conflicts, invoke the `resolving-conflicts` skill.
5. Stop and confirm the changes with the user. Ask them to review the changes and wait for confirmation to proceed.
6. Submit the PR using the [template](./reference/pull-request-template.md).
