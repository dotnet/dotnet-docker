---
name: ".NET Core Release"
about: Tracks information for managing a .NET Core release
title: ".NET Core Release"
labels: ''
assignees: ''

---

# .NET Core Release

## Release Versions
_The set of .NET Core versions that are being released as a unit._

* runtime/SDK

## Update Dependencies Commands
_The set of commands to run in a dev environment which will update all the necessary files to reflect the specified .NET Core versions._

* `dotnet run --project .\eng\update-dependencies\update-dependencies.csproj --sdk-version <sdk> --runtime-version <runtime> --aspnet-version <runtime>`

## PRs/Commits to Merge
_The PRs/commits that need to be merged to the `master` branch prior to executing a release build._

- [ ] link
