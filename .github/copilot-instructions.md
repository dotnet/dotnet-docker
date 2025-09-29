# Instructions for GitHub Copilot

## How to edit Dockerfiles and READMEs

- Dockerfiles and READMEs are generated from templates using Cottle.
- Dockerfile templates are located in the `eng/dockerfile-templates` directory
- README templates are located in the `eng/readme-templates` directory.
- Do not edit the Dockerfiles in `src/` directly.
- To generate Dockerfiles from templates, run `pwsh ./eng/dockerfile-templates/Get-GeneratedDockerfiles.ps1`.
- Do not edit `*README*.md` files directly.
- To generate READMEs, run `pwsh ./eng/readme-templates/Get-GeneratedReadmes.ps1`.

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

- To add new images, follow the instructions in the [new image checklist](./ISSUE_TEMPLATE/releases/new-image-release.md).
- To add a new Linux distro, follow the instructions in the [new distro checklist](./ISSUE_TEMPLATE/releases/new-distro-release.md).

## Samples

- Sample Dockerfiles and README files are not generated from templates.
