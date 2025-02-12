{{
    _ Wrapper template for providing the list of repos to other templates.

    _ ARGS:
      template: Template to pass the repo lists to.
        All other args will be passed to the template. ^

    set argsToForward to except(ARGS, [ "template": ARGS["template"] ]) ^

    set productRepos to [
        ["dotnet/sdk", ".NET SDK"],
        ["dotnet/aspnet", "ASP.NET Core Runtime"],
        ["dotnet/runtime", ".NET Runtime"],
        ["dotnet/runtime-deps", ".NET Runtime Dependencies"],
        ["dotnet/monitor", ".NET Monitor Tool"],
        ["dotnet/monitor/base", ".NET Monitor Base"],
        ["dotnet/aspire-dashboard", ".NET Aspire Dashboard"]
    ] ^
    set nightlyOnlyRepos to [
        ["dotnet/yarp", "YARP (Yet Another Reverse Proxy)"]
    ] ^
    set productFamilyRepos to [
        ["dotnet", ".NET", 1]
    ] ^
    set samplesRepos to [
        ["dotnet/samples", ".NET Samples"]
    ] ^
    set frameworkRepos to [
        ["dotnet/framework", ".NET Framework, ASP.NET and WCF", 1],
        ["dotnet/framework/samples", ".NET Framework, ASP.NET and WCF Samples"]
    ]

}}{{InsertTemplate(ARGS["template"], union(argsToForward, [
    "product-repos": productRepos,
    "nightly-only-repos": nightlyOnlyRepos,
    "product-family-repos": productFamilyRepos,
    "samples-repos": samplesRepos,
    "framework-repos": frameworkRepos
]))}}
