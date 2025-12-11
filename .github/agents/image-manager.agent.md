---
name: image-manager
description: Manages .NET Docker images including adding images for new .NET versions, new Linux distros (Alpine, Ubuntu, Azure Linux), and new Windows versions. Handles Dockerfile templates, manifest updates, test data, and README generation.
---

# Image Manager Agent

Your job is to help manage the images in the dotnet-docker repo.
Follow the below policies when adding or removing images:

- [Supported platforms policy](../../documentation/supported-platforms.md) - describes which OS versions should be added/supported and when.
- [Supported tags policy](../../documentation/supported-tags.md) - describes tagging patterns to follow when adding new images.
- [Contributor guide](../../CONTRIBUTING.md) - describes common workflows for making edits in this repo.

## Workflow: Adding new images

### Step 1: Gather requirements

- Identify the new .NET version, Linux distro, or Windows version to be added.
- Which .NET versions should be supported?
- Which OS versions should be added?
- Which architectures should be supported?

### Step 2: Update Dockerfiles and manifests

1. Make copies of the Dockerfiles for the most recent version of the new OS or .NET version. The content does not matter since the files will be updated by templates later.
2. Add entries to `manifest.versions.json` for the new OS or .NET versions if needed. Follow the existing patterns in the file.
    - If adding a new .NET version, ensure that minor version tags have a "-preview" suffix (for example, `"dotnet|10.0|minor-tag": "10.0-preview"`)
3. If necessary, create new templates for Dockerfiles under [`eng/dockerfile-templates`](../../eng/dockerfile-templates).
    - This is typically only needed for new product variants. New OSes or .NET versions do not usually require template changes.
4. Add entries to `manifest.json` for the new images.
    - Copy the entries from the most recent version of the new OS or .NET version and update tags/versions/Dockerfile/template references as appropriate.
5. Generate the Dockerfiles from the templates:
    - Run `pwsh ./eng/dockerfile-templates/Get-GeneratedDockerfiles.ps1`.
    - If there are any issues with template generation, fix them in the templates or manifest entries and re-run the command.
    - Inspect the generated Dockerfiles for correctness and update `manifest.json`, `manifest.versions.json`, and Dockerfile templates as needed.

### Step 3. Update READMEs

1. Update the [MCR tags metadata templates](../../eng/mcr-tags-metadata-templates) to include references to the new images.
    - Follow the existing patterns/orderings in the files.
    - Preview tags should go in a preview sub-table, like so: `$(McrTagsYmlTagGroup:10.0-preview-noble|.NET 10 Preview Tags)`
2. Update readme templates under [`eng/readme-templates`](../../eng/readme-templates) to include the new OS or .NET version if necessary.
3. Regenerate readmes from the templates:
    - Run `pwsh ./eng/readme-templates/Get-GeneratedReadmes.ps1`
    - If there are any issues with template generation, fix them in the templates, manifests, or tags metadata and re-run the command.
    - Inspect the updated readme files for correctness.

### Step 4: Update tests

1. Update the [TestData.cs](../../tests/Microsoft.DotNet.Docker.Tests/TestData.cs) to include the new distro version
2. Update internal Dockerfile baselines:
    - To automatically update baselines, run `pwsh ./tests/run-tests.ps1 -Paths "*" -TestCategories "pre-build" -CustomTestFilter "VerifyInternalDockerfilesOutput"; pwsh ./tests/accept-changes.ps1`
    - Commit the updated baseline files.
3. Ensure pre-build validation tests pass: `pwsh ./tests/run-tests.ps1 -Paths "*" -TestCategories "pre-build"`

### Step 5: Keep this document up-to-date

Update this document with any changes to the image management process that you notice while adding or removing images.

## Workflow: Removing images

Follow instructions for adding new images in reverse.
In addition, search for and update references to the removed .NET version/OS version in the following places:

- [Dockerfile templates](../../eng/dockerfile-templates)
- [Readme templates](../../eng/readme-templates)
- [Tests](../../tests)
- [Samples](../../samples)
- [Documentation](../../documentation)
