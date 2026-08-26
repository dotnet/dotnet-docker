---
name: porting-changes
description: >-
  Plan and move changes between branches in dotnet/dotnet-docker.
  Use when asked to port or backport changes.
---

# Porting changes between branches

## Before porting

- Identify the source branch, target branch, and reason for the port.
- Read the [branch guide](../../../eng/developer-docs/branches.md).
- If either branch is a release branch, use `pwsh eng/Get-ReleaseBranches.ps1` to identify it. Do not assume `main` is the current release branch.
- Compare branch history, source PRs, and final patches. A different commit may already provide the same change on the target.
- Identify the logical changes and their dependencies. Include only changes that belong on the target.

Use these common directions as a guide:

| Direction | Port when |
| --- | --- |
| `nightly` to `release/*` | A change in nightly is planned to be shipped in the next release. This is the most common porting operation. Exclude daily-only version bumps, experiments, and changes already represented on the release branch. |
| `release/*` to `main` | The release contents are final and will be published soon. |
| `main` to `nightly` | After each public release, merge all of `main` into `nightly`. Also see the `merge-main-to-nightly` skill. |
| `nightly` to `main` | A fix in the `nightly` branch affects all currently published images, shared automation, tests, samples, or documentation before the next release merge. |
| `main` to `release/*` | `main` had changes after the release branch was created. Example: new Aspire Dashboard version. |
| `release/*` to `nightly` | Rare. Prefer the normal `release/*` to `main` to `nightly` route. Port directly only when nightly needs an urgent fix before that route completes. |

Version-only daily or preview updates stay on `nightly`, except when that exact preview version is being published or when those updates include meaningful changes beyond just versions (Dockerfile changes, test changes, etc.).
Security, correctness, and release-blocking fixes may take priority over the normal cadence.

## Choosing an integration method

- The default flow is to cherry-pick the final (squashed or merged) commits for each change/PR directly from the source branch.
- If the change does not apply cleanly, confirm the expected behavior with the user. The change may require extra work to preserve the user's intention.

## Applying changes

- Base new work on the target branch.
- Preserve product versions, publishing settings, and other state that intentionally differs on the target.
- Follow the `resolving-conflicts` skill when a cherry-pick or merge has conflicts.
- Regenerate Dockerfiles, READMEs, and test baselines after changing their inputs.

## Completing the port

1. Review the complete target diff and run focused validation.
2. Create a *draft* PR using the [PR template](./reference/pull-request-template.md).
