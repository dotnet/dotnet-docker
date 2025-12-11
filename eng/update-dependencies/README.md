# update-dependencies

This folder contains the `update-dependencies` tool that automates updating
component versions for images built from this repo. It is used to:

- Update versions recorded in `manifest.versions.json`.
- Regenerate Dockerfiles and READMEs from their templates (via the build
  scripts) using those updated versions.
- Create / update a pull request with the changes.

## Implementation

The tool has two layers of implementation code:

1. Modern CLI built with `System.CommandLine` and dependency injection.
   These commands retrieve version information from official .NET build assets
   (BAR builds/channels, staging pipeline artifacts) and then map that
   information into a standardized set of product versions.
2. Legacy code built on `Microsoft.DotNet.VersionTools.Automation` that
   performs regex replacement of versions, checksums, and URLs in
   [manifest.versions.json](../../manifest.versions.json). This logic is
   encapsulated behind `SpecificCommand` and related helper classes
   (e.g. `VersionUpdater`, `DockerfileShaUpdater`).

Currently, the modern CLI front-end of the tool resolves product versions and
hands them over to the `SpecificCommand` class to perform file updates. New
features should only be added to the modern CLI front-end. `SpecificCommand`
will be removed/re-implemented eventually since it is tightly coupled to an
unsupported package dependency (`Microsoft.DotNet.VersionTools.Automation` -
removal tracked by [this GitHub issue](https://github.com/dotnet/docker-tools/issues/1658)).

## Adding support for new dependencies

### .NET dependencies

To add support for new .NET repos to the `from-build` and `from-channel`
commands:

- Add the new repo to the [`BuildRepo`](./BuildRepo.cs) enum.
- Update the [GetBuildRepo()](./BuildExtensions.cs) extension method to support
  the new enum value.
- Implement a new [`IBuildUpdaterService`](./IBuildUpdaterService.cs), using
  existing implementations as a reference.
- Register a new keyed singleton in [Program.cs](./Program.cs) using the new
  repo enum value as the key.

If the dependency can be resolved from a pipeline artifact, GitHub release,
static file, or something similar, then you may be able to treat it like an
[external dependency](#external-projects).

### External projects

1. Add your component's version to
   [manifest.versions.json](../../manifest.versions.json) with the format:
   `<product-key>|<dockerfile-version>|build-version`. For example:
   `chisel|10.0|build-version` or `mingit|latest|build-version`. This version
   can be used directly in Dockerfile templates or can be used to compose other
   variables in the versions file.
1. Create a new class that implements
   [IDependencyVersionSource](./IDependencyVersionSource.cs). You can use
   whatever method you need to fetch version updates. Example implementation:
   [ChiselVersionSource](./ChiselVersionSource.cs) (determines version using
   GitHub Releases).
1. Register your IDependencyVersionSource implementation in
   [Program.cs](./Program.cs) using **the same product key** from step 1.
1. You can now update your product's version by running the following command:
   `dotnet run --project ./eng/update-dependencies/update-dependencies.csproj -- from-component <dockerfile-version> <product-key>`
1. (Optional) Add a job to the
   [update-dependencies pipeline](../pipelines/pipelines/update-dependencies.yml)
   to enable automatic update pull requests.
