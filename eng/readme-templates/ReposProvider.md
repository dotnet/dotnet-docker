{{
    _ Wrapper template for providing the list of repos to other templates.

    _ ARGS:
      top-header: The string to use as the top-level header.
      readme-host: Moniker of the site that will host the readme
      template: Template to pass the repo lists to ^

    set repos to [
        ["dotnet/sdk", ".NET SDK"],
        ["dotnet/aspnet", "ASP.NET Core Runtime"],
        ["dotnet/runtime", ".NET Runtime"],
        ["dotnet/runtime-deps", ".NET Runtime Dependencies"],
        ["dotnet/monitor", ".NET Monitor Tool"],
        ["dotnet/monitor/base", ".NET Monitor Base"],
        ["dotnet/aspire-dashboard", ".NET Aspire Dashboard"]
    ] ^
    set commonRepos to [
        ["dotnet", ".NET", 1],
        ["dotnet/samples", ".NET Samples"]
    ] ^
    set frameworkRepos to [
        ["dotnet/framework", ".NET Framework, ASP.NET and WCF", 1],
        ["dotnet/framework/samples", ".NET Framework, ASP.NET and WCF Samples"]
    ]

}}{{InsertTemplate(ARGS["template"], [
    "top-header": ARGS["top-header"],
    "readme-host": ARGS["readme-host"],
    "repos": repos,
    "common-repos": commonRepos,
    "framework-repos": frameworkRepos
])}}