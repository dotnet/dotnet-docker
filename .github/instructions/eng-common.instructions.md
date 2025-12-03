---
applyTo: "eng/docker-tools/**/*"
---

# Instructions for modifying files in eng/docker-tools/

Do not edit files in `eng/docker-tools/`. These files are shared across multiple
repositories and are managed in the [dotnet/docker-tools](https://github.com/dotnet/docker-tools)
repository.  Changes must be made in that repository and then propagated to
this repository via a pull request.

If you are working on a particularly complex infrastructure change, you may
choose to make local changes to `eng/docker-tools/`, but make sure to notify the user
that the changes must be backported to the `dotnet/docker-tools` repository.
