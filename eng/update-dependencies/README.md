# update-dependencies

This folder contains the `update-dependencies` tool that automates updating
component versions for images built from this repo. It is used to:

- Update versions recorded in `manifest.versions.json`.
- Automatically regenerate Dockerfiles and READMEs from their templates.
- Create or update pull requests with the changes.

## Usage

Run the tool to see available commands:

```console
dotnet run --project eng/update-dependencies -- --help
```

For implementation details and instructions for adding new dependencies, see
[AGENTS.md](./AGENTS.md).
