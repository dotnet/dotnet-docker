---
name: backport-changes
description: Analyze and backport changes from nightly to the release branch in dotnet/dotnet-docker.
user-invocable: true
disable-model-invocation: true
---

# Backporting Changes

## Workflow

1. **Determine the release branch**: Run `pwsh ../shared/Get-ReleaseBranches.ps1` to find the latest public release branch.
   - The most recently created public release branch corresponds to the current release.
   - Do not assume `main` is the release branch.
2. **Get candidates for backport**: Run `pwsh scripts/Get-BackportPRs.ps1` to find PRs to backport.
3. **Analyze PRs** - classify each PR using the backport guidelines below and present the analysis table to the user.
4. **Cherry-pick** - confirm the plan with the user, create a working branch based off of the public release branch, then run `git cherry-pick <commits>` (in the order they were merged).
    - If any templates or manifests changed, regenerate Dockerfiles and READMEs. Confirm that the diff looks correct.
5. **Resolve conflicts** - follow the conflict resolution table below; stop and consult the user for anything not covered.
6. **Verify nothing was missed** - from your working branch with all cherry-picks committed and a clean working tree, run `pwsh scripts/Get-BackportDiff.ps1` to diff `HEAD` against `nightly`. Confirm every reported difference is an expected divergence (see below). Investigate anything that is not.
7. **Open the PR** - push the working branch and open a PR targeting the public release branch.
8. **Clean up labels** - remove the `needs-backport` label from each backported PR by running `pwsh scripts/Remove-BackportLabels.ps1 -PRs <numbers>`. Leave the label on PRs you intentionally did not backport.

## Backport guidelines

- Backport:
  - New images - if ready for the main branch
  - Removal of EOL images - end-of-life image cleanup
  - Dockerfile/template changes - structural changes to how images are built
  - Image component updates - MinGit, PowerShell, and other tools
  - Infrastructure and tooling changes - build scripts, CI/CD updates
  - Automated `eng/docker-tools` updates - standard engineering infrastructure
- Do not backport:
  - Version-only updates for daily/preview builds (no Dockerfile changes)
  - Changes already on the release branch
  - Merge commits (e.g. "Merge main to nightly") - that content reaches the release branch via the main cut, not the backport
  - Experimental or incomplete features
- Requires extra consideration:
  - Daily builds of .NET or appliance images - only backport if they include Dockerfile changes beyond simple version updates.

## Output

After analyzing PRs, present results in a table:

```markdown
| PR | Title | Backport | Reason | Commit |
| --- | --- | --- | --- | --- |
| #1234 | Update PowerShell to X.Y.Z | ✅ Yes | Component update | 123abc... |
| #1235 | Daily build .NET X.Y.Z-preview.Y | ❌ No | Version-only update, no Dockerfile changes | 123abc... |
| #1236 | Add Ubuntu X.Y images | ✅ Yes | New images ready for release | 123abc... |
```

## Conflict resolution

| Conflicting files | Resolution |
| --- | --- |
| `src/*` | Regenerate Dockerfiles. |
| `READMEs` | Regenerate READMEs. |
| `manifest.json` | Take changes from nightly, ensure `latest` tags are on the correct (non-preview) versions, then regenerate Dockerfiles and READMEs. |
| `manifest.versions.json` | Keep the latest/most up-to-date versions. |
| Other files | Stop and consult the user. |

## Expected divergences from nightly

When verifying (step 6), the following differences between the release branch and `nightly` are expected and do **not** indicate a missed backport. Note that a single in-progress feature on `nightly` usually shows up as a *cluster* of related files (Dockerfiles, templates, tag metadata, version entries, and test baselines) — recognize the feature, don't classify file-by-file.

- **Preview version drift** - `nightly` carries newer daily/preview build numbers that are intentionally not backported. Shows up across `src/*` Dockerfiles, `README*.md`, `eng/mcr-tags-metadata-templates/*`, and version entries in `manifest.versions.json`.
- **Appliance image versions** (aspire-dashboard, monitor, yarp) - updated separately during release staging, not via backport. New appliances still in preview (e.g. yarp) may be absent from the release branch entirely.
- **New images or distros still in development on `nightly`** - e.g. support for a new pre-release Linux distro. These appear as a coupled set: new `src/*` Dockerfiles, structural `eng/dockerfile-templates/*` changes, new `eng/mcr-tags-metadata-templates/*` tag groups, `*|repo` entries in `manifest.versions.json`, and added/removed `VerifyInternalDockerfilesOutput` baselines. Expected only while the feature is not yet shipping in this release — otherwise investigate as a missed backport (see below).
- **Internal Dockerfile test baselines** - `tests/.../Baselines/GeneratedArtifactTests/VerifyInternalDockerfilesOutput/*` churn (added/removed/modified) tracks the image set above and the internal build flavor; expected alongside the image and template differences.
- **Out-of-release-scope content** - files that are never part of a servicing backport, such as `samples/*` (not generated, not part of the released image set). Differences here are expected. This is **not** a catch-all for "anything only on `nightly`" — nightly content that belongs in the release (new images ready to ship, component updates, tooling/infra, structural template changes) is precisely what a backport must carry, so treat it as a potential miss unless it falls under one of the categories above.
- **`manifest.versions.json` `branch` field** - `main` on the release branch vs `nightly`.
- **`manifest.json` repo remapping** - the release branch uses public repo names while `nightly` uses nightly ones (e.g. `dotnet/nightly/*` → `dotnet/*`, `core-nightly` → `core`, including the `syndicated*Repo` variables).

Anything outside these categories should be investigated as a potential missed backport. Structural `eng/dockerfile-templates/*` changes are only expected when they are tied to a feature that is intentionally not shipping in this release; an isolated template change with no accompanying in-development feature normally **should** be backported.
