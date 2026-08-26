---
name: resolving-conflicts
description: Use when merging or cherry-picking changes cause git conflicts.
---

# Resolving branch conflicts

1. Name the source and target branches. Read the [branch guide](../../../eng/developer-docs/branches.md) before choosing either side.
2. Remember that `ours` and `theirs` depend on the Git operation. Inspect the stages with `git ls-files -u`; never resolve a file based only on those labels.
3. Resolve authored inputs first, then regenerate derived files.

| Conflict | Resolution |
| --- | --- |
| `manifest.versions.json` | Start with the target's branch and product versions. Apply only the source component/version keys required by the port. "Newest" is not a rule: nightly daily builds and release servicing builds have different intent. |
| `manifest.json` | Combine structural image changes, but preserve the target's image set, repositories, `latest` ownership, floating tags, and preview suffixes. |
| `eng/dockerfile-templates/**` | Keep shared build logic from both changes. Retain a difference only when the target builds a different product or OS matrix. |
| `src/**/Dockerfile` | Do not hand-merge generated Dockerfiles. Resolve manifests and templates, then regenerate them. |
| README and portal docs | Resolve README templates and manifests, then regenerate. Preserve target-specific official or nightly links and tag listings. |
| Tests and baselines | Port shared behavior tests. Preserve the target product matrix and regenerate baselines after resolving source. |
| Pipelines and workflows | Combine shared logic; preserve target-specific triggers, branch names, feeds, service connections, staging, and publishing behavior. |
| Modify/delete conflicts | Decide whether the image or feature belongs on the target branch. Do not restore or delete it merely to clear the conflict. |
| Other authored files | Merge the behavior semantically. A clean conflict marker resolution is not proof that the result is correct. |

Always regenerate Dockerfiles and READMEs when templates change.

Review `git diff --check`, generated-file consistency, tags, versions, image sets, and branch references. Run targeted pre-build tests. If the intended target behavior is unclear, stop and record the unresolved decision instead of guessing.
