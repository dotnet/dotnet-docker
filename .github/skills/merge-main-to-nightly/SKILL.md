---
name: merge-main-to-nightly
description: Create a PR to merge main into the nightly branch after a .NET containers servicing release. Use when updating the nightly branch post-release or merging main to nightly.
user-invocable: true
disable-model-invocation: true
---

# Merge main branch to nightly branch

## Workflow

1. **Determine the release name**: Run `pwsh ../shared/Get-ReleaseBranches.ps1` to find the latest release branch.
   - The most recently created branch corresponds to the current release.
2. **Create a working branch** - create a new branch based off of the `nightly` branch, called `main-to-nightly-$releaseName`.
   - `git fetch upstream nightly && git checkout -b main-to-nightly-$releaseName upstream/nightly`
3. **Merge main into the branch** - merge the `main` branch into your new branch, fixing any merge conflicts as necessary.
   - `git fetch upstream main && git merge upstream/main`
   - If there are merge conflicts, refer to the conflict resolution table below.
4. **Confirm with the user** - Let the user review the changes and the diff.
5. **Submit the PR** - Push the branch to `origin` and create a PR targeting the `nightly` branch.

## Conflict resolution

| Conflicting files | Resolution |
|---|---|
| `src/*` | Regenerate Dockerfiles. |
| `READMEs` | Regenerate READMEs. |
| `manifest.json` | Take changes from nightly, ensure preview/nightly tags remain, then regenerate Dockerfiles and READMEs. |
| `manifest.versions.json` | Keep nightly's versions (which are typically newer/preview). Incorporate any structural changes from main. |
| Other files | Stop and consult the user. |
