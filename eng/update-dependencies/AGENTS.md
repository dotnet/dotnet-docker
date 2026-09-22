# Instructions for update-dependencies

For usage, see [README.md](./README.md).

## Design/Concepts

- An [Updater](./Updaters) contains all details needed to update one specific
  product. It implements `IUpdater` and one or more of the other `I.*Updater`
  interfaces.
- [Commands](./Commands) map to the sources for dependency updates. These are
  typically implemented as static `Command` factories that bind to command line
  arguments. Commands know how to pass the correct values to the corresponding
  `I.*Updater` interface.
  - If the command fits within the standard updater/command split, it should be
    registered in [DependencyCommands.cs](./Commands/DependencyCommand.cs).
    The exception is if it's a one-off workflow type of command, like
    [SyncInternalReleaseCommand](./Commands/SyncInternalReleaseCommand.cs).

Example:

- `FromBuildOptions`: declares the `System.CommandLine` options/arguments for
  one specific BAR build ID.
- `IBarBuildUpdater`: declares a method that takes the necessary parameters to
  update from a BAR build.
- `FromBuildCommand`: maps `FromBuildOptions` (command line options) to
  `IBarBuildUpdater` method parameters.
- `DotnetUpdater`: implements `IBarBuildUpdater`.

In this way, the tool separates **what to update** from **where versions come
from**. `DotnetUpdater` and `FromBuildCommand` do not need to know about each
other. Adding a new dependency from an existing update source would only
require implementing an `I.*Updater` interface. Implementing a new update
source only requires adding a new updater implementation if there is something
fundamentally different about how the update source maps to the existing
updater interfaces.

## Command line

Updaters and commands are automatically composed together in the CLI without any
extra work: `update-dependencies <updater> <command>`.

## Inputs/Configuration

Inputs come as either [app configuration](./appsettings.json) (via
Microsoft.Extensions.Configuration) or from the command line.

- Use configuration for things that do not change between runs or between
  different commands: Azure DevOps org/project, GitHub repo, secrets, etc.
- Use command line args for inputs that vary between each run, like build IDs,
  channels, branch targets, or versions.
- Basically, use command line args for anything that you might want to change
  via pipeline parameters/variables without submitting a pull request.

## Goals to preserve

- Keep each dependency's update rules together so they do not diverge between
  sources.
- Support only combinations that make sense for a dependency. Composition does
  not mean every updater must support every command.
- Separate selecting an update from applying it so the same update can work in
  a local checkout or a temporary workspace used to prepare a pull request.
