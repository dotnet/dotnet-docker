---
name: backport-changes
description: Analyze and backport changes from nightly to the release branch in dotnet/dotnet-docker.
user-invocable: true
disable-model-invocation: true
---

# Backporting Changes

## Workflow

1. **Get candidates for backport**: Run `pwsh scripts/Get-BackportPRs.ps1` to find PRs to backport.
2. **Analyze PRs** - classify each PR using the backport guidelines below and present the analysis table to the user
3. **Cherry-pick** - confirm the plan with the user, then run `git cherry-pick <commits>` (in the order they were merged).
    - If any templates or manifests changed, regenerate Dockerfiles and READMEs. Confirm that the diff looks correct.
4. **Resolve conflicts** - follow the conflict resolution table below; stop and consult the user for anything not covered

## Backport guidelines

- Backport:
  - New images - if ready for the main branch
  - Removal of EOL images - end-of-life image cleanup
  - Dockerfile/template changes - structural changes to how images are built
  - Image component updates - MinGit, PowerShell, and other tools
  - Infrastructure and tooling changes - build scripts, CI/CD updates
  - Automated `eng/common` updates - standard engineering infrastructure
- Do not backport:
  - Version-only updates for daily/preview builds (no Dockerfile changes)
  - Changes already on the release branch
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
