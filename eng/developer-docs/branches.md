# Branches

This repository uses separate branches for daily development, public releases, and internal release testing.

## Branch roles

| Branch | Purpose |
| --- | --- |
| `main` | Builds the official .NET images. It contains released and public preview versions. Samples and public documentation start here. |
| `nightly` | Builds pre-release versions images. Most source changes start here. It contains daily builds of .NET and other experimental changes. |
| `release/YYYY-MMB` | Staging ground for changes from `nightly` before they are merged into main. It branches from `main` and is merged back into `main` on release day. |
| `internal/release/YYYY-MMB` | Same as the `release/YYYY-MMB` branch, but may contain internal pre-release .NET versions. Only lives in Azure DevOps, never GitHub. |

## Differences between `main` and `nightly`

The branches share image logic and infrastructure. The following files may differ when the products or publishing targets differ.

| Area | Allowed difference | Rule |
| --- | --- | --- |
| `manifest.versions.json` | Product versions, checksums, base images, and download URLs | Keep released products on `main`. `nightly` may use daily or unreleased products. |
| `manifest.json`<br>`manifest.samples.json` | Image set, tags, repository names, and publishing metadata | Each difference must belong to a public milestone or an active nightly experiment. |
| `src/**/Dockerfile`<br>`README*.md`<br>`.portal-docs/**` | Generated content caused by manifest, version, or template differences | Change the source template or manifest, then regenerate the files. Do not keep generated-only changes. |
| `eng/dockerfile-templates/**` | Dockerfile logic needed by the products and image variants on each branch | Share template logic unless a branch-specific product or image requires a difference. |
| `eng/mcr-tags-metadata-templates/**` | Tags, `latest` tag ownership, preview labels, and repository metadata | Keep official publishing behavior on `main` and daily publishing behavior on `nightly`. |
| `eng/readme-templates/**` | Documentation needed by the products and images on each branch | Share template text unless branch-specific products or images require a difference. |
| `tests/**` | Version, operating system, tag, and baseline data | Share test code and assertions. Keep only product-specific test data different. |
| `eng/pipelines/**` | Triggers, feeds, service connections, staging settings, and publishing targets | Share pipeline logic. Keep only environment-specific settings different. |
| `samples/**`<br>`documentation/**` | Samples and public documentation normally exist on `main` first | Port a change to `nightly` when daily images or tests need it. |

## Allowed internal release branch differences

The internal release branch may differ from its public release branch only in the following paths:

| Area | Internal branch content |
| --- | --- |
| `manifest.versions.json` | Staged .NET versions, checksums, and download URLs |
| `stage-containers.txt` | Staging containers used to reapply internal product updates after a sync |
| `src/**/Dockerfile`<br>`README*.md`<br>`.portal-docs/**`<br>`tests/**` | Generated files and test data produced from staged versions and URLs |
| `eng/pipelines/**` | A temporary staging workaround with a linked cause and removal condition |

All other changes should be made directly on the public release branch.
The [sync-internal-release-official](../pipelines/sync-internal-release-official.yml) pipeline automatically syncs the public release branch to the internal release branch by submitting a pull request in Azure DevOps.

Use the [update-dependencies-internal-official](../pipelines/update-dependencies-internal-official.yml) pipeline to apply internal pre-release .NET versions to the internal release branch.
The [release-staging-official](../pipelines/release-staging-official.yml) pipeline automatically runs whenever changes are made to the internal release branch.

## Release flow

1. Create `release/YYYY-MMB` from `main`, then port the required release changes from `nightly`. Invoke the `porting-changes` skill for this.
2. Let the [sync-internal-release-official](../pipelines/sync-internal-release-official.yml) pipeline copy the public release branch to `internal/release/YYYY-MMB`.
3. Apply internal pre-release builds to the internal release branch using the [update-dependencies-internal-official](../pipelines/update-dependencies-internal-official.yml) pipeline.
4. Ensure the [release-staging-official](../pipelines/release-staging-official.yml) has a successful run. This means the images are ready for release.
5. When the new .NET versions are released, run the [release-promotion-official](../pipelines/release-promotion-official.yml) pipeline to publish the staged container images.
6. Merge the public release branch into `main`. Include new product versions by running [update-dependencies](../update-dependencies/) tool.
7. Merge `main` into `nightly` without squashing. Invoke the `merge-main-to-nightly` skill for this.
