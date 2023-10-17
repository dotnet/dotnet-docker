{{
    _ ARGS:
      top-header: The string to use as the top-level header.
      readme-host: Moniker of the site that will host the readme
}}{{InsertTemplate("Use.runtime.md", [
    "repo": "dotnet/samples",
    "readme-host": ARGS["readme-host"],
    "top-header": ARGS["top-header"]
])}}

{{InsertTemplate("Use.aspnet.md", [
    "repo": "dotnet/samples",
    "readme-host": ARGS["readme-host"],
    "top-header": ARGS["top-header"]
])}}
