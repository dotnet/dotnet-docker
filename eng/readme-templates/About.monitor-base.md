{{
    _ ARGS:
      readme-host: Moniker of the site that will host the readme ^
    set monitorRepo to when(VARIABLES["branch"] = "nightly", "dotnet/nightly/monitor", "dotnet/monitor") ^
    set useRelativeLink to ARGS["readme-host"] = "github"
}}This image contains the base installation of .NET Monitor, a diagnostic tool for capturing diagnostic artifacts (such as dumps and traces) in an operator-driven or automated manner. This tool is an ASP.NET application that hosts a web API for inspecting .NET processes and collecting diagnostic artifacts.

This image only provides the base functionality of the .NET Monitor tool; it is only meant to be used as a base image upon which .NET Monitor extensions are installed. If you are looking for the full feature set that is provided by the .NET Monitor global tool (including the egress capabilities), see the [{{monitorRepo}}]({{InsertTemplate("Url.md", [ "repo": monitorRepo "readme-host": ARGS["readme-host"] "use-relative-link": useRelativeLink ])}}) image.
