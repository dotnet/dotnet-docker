# update-dependencies

This folder contains the `update-dependencies` tool that automates updating
component versions for images built from this repo. It is used to:

- Update versions recorded in `manifest.versions.json`.
- Regenerate Dockerfiles and READMEs from their templates (via the build
  scripts) using those updated versions.
- Create / update a pull request with the changes.

## Implementation

Commands built with `System.CommandLine` and dependency injection retrieve
version information from BAR builds/channels, staging pipeline artifacts, or
component version sources. They pass product versions or resolved manifest
values to [SpecificCommand](./SpecificCommand.cs).

`SpecificCommand` owns the update flow. It loads a shared
[ManifestVariables](./ManifestVariables.cs) editor and directly calls the
product and tool update functions. These functions keep their own selection
policies, such as preserving aliases or empty values. Product versions and
base URLs are updated before checksum retrieval so later operations can read
the new values from the same editor. NuGet configuration is updated separately.

The command writes [manifest.versions.json](../../manifest.versions.json) only
if its content changed, preserving formatting outside edited values. It then
runs the Dockerfile and README generators, even when the manifest is unchanged.
Update and generation failures propagate to the command boundary.

In local-only mode, updates run in the supplied checkout.
[DependencyUpdatePublisher](./DependencyUpdatePublisher.cs) uses
`Microsoft.DotNet.GitAutomation` to run the same updates in a publishing
workspace and create or update a pull request. GitAutomation detects changes
across the repository, including generated files and NuGet configuration,
rather than relying on manifest changes alone.

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
