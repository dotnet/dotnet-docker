# Instructions for GitHub Copilot

## How to edit Dockerfiles and READMEs

- Dockerfiles and READMEs are generated from templates using Cottle.
- Dockerfile templates are located in the `eng/dockerfile-templates` directory
- README templates are located in the `eng/readme-templates` directory.
- Do not edit the Dockerfiles in `src/` directly.
- To generate Dockerfiles from templates, run `pwsh ./eng/dockerfile-templates/Get-GeneratedDockerfiles.ps1`.
- Do not edit `*README*.md` files directly.
- To generate READMEs, run `pwsh ./eng/readme-templates/Get-GeneratedReadmes.ps1`.
- Use the [`dockerfile-and-readme-templating`](.agents/skills/dockerfile-and-readme-templating/SKILL.md) skill when modifying their Cottle templates.

## Manifests

- `manifest.json` describes the Dockerfiles in this repo and how they should be built, tagged, and published.
- `manifest.versions.json` contains product version information used by the Dockerfile templates. It is typically updated using the `eng/update-dependencies` tool.

## How to build and test

- Only build and test images that were changed.
- When changing many images, just build and test a single combination of .NET version and OS as a sanity check.
- To build Dockerfiles, run `pwsh ./build-and-test.ps1 -mode 'Build' -paths '*glob*pattern*'`. For example, to build all .NET 9.0 Ubuntu Noble images, run `./build-and-test.ps1 -paths '*9.0*noble*'`.
- To run image tests, run `pwsh ./tests/run-tests.ps1 -paths '*glob*pattern*'`.
- To run only the pre-build validation tests, run `pwsh ./tests/run-tests.ps1 -paths '*' -TestCategories @('pre-build')`.

## How to add images

- To add new images or a new Linux distro, use the [`image-management`](.agents/skills/image-management/SKILL.md) skill.

## How to work across branches

- Use the [`porting-changes`](.agents/skills/porting-changes/SKILL.md) skill to port or backport changes between branches.
- Use the [`resolving-conflicts`](.agents/skills/resolving-conflicts/SKILL.md) skill when a merge or cherry-pick has conflicts.
- Use the [`merge-main-to-nightly`](.agents/skills/merge-main-to-nightly/SKILL.md) skill when asked to merge `main` into `nightly` after a release.

## Samples

- Sample Dockerfiles and README files are not generated from templates.
