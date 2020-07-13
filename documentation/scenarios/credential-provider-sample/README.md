# Azure Artifact Credential Provider Sample

This sample project provides an example of how to configure a .NET Core project that references a NuGet package contained in an Azure Artifacts feed. For more information on authenticating to NuGet feeds in a Dockerfile, see [Managing NuGet Credentials in Docker Scenarios](../nuget-credentials.md).

This sample project is not intended to be buildable as it is defined since it references a ficticious Azure Artifacts feed. It should be used as a reference to understand how to configure a simple project in a Docker scenario. It can also be used as a helpful template to verify the ability of a Dockerfile that, when built, can successfully authenticate to your Azure Artifacts feed. Follow the instructions below on how to do this.

## Verify connection to your own feed

1. Clone [dotnet/dotnet-docker](https://github.com/dotnet/dotnet-docker):

    ```bash
    git clone https://github.com/dotnet/dotnet-docker.git
    ```

1. Go to the sample project's directory:

    ```bash
    cd documentation/scenarios/credential-provider-sample
    ```

1. Edit the [nuget.config](https://github.com/dotnet/dotnet-docker/tree/master/documentation/scenarios/credential-provider-sample/nuget.config) file and either the [Dockerfile.linux](https://github.com/dotnet/dotnet-docker/tree/master/documentation/scenarios/credential-provider-sample/Dockerfile.linux) or [Dockerfile.windows]((https://github.com/dotnet/dotnet-docker/tree/master/documentation/scenarios/credential-provider-sample/Dockerfile.windows)) file, depending on which Docker environment you're targeting:

   a. Replace `https://fabrikam.pkgs.visualstudio.com/_packaging/MyGreatFeed/nuget/v3/index.json` with your feed URL.

1. Edit the [dotnetapp.csproj](https://github.com/dotnet/dotnet-docker/tree/master/documentation/scenarios/credential-provider-sample/dotnetapp.csproj) file:

   a. Replace `<PackageReference Include="FabrikamLib" Version="1.0.0" />` with a reference to a NuGet package from the feed you're referencing.

1. Set an environment variable to the [personal access token](https://docs.microsoft.com/azure/devops/organizations/accounts/use-personal-access-tokens-to-authenticate) (PAT) for the feed you're referencing, replacing `<PAT>` with the value of the PAT:

   * Linux shell:

     ```bash
     export FEED_ACCESSTOKEN="<PAT>"
     ```

   * Windows CMD:

     ```bash
     set FEED_ACCESSTOKEN="<PAT>"
     ```

   * Windows PowerShell:

     ```powershell
     $Env:FEED_ACCESSTOKEN="<PAT>"
     ```

1. Build the Dockerfile:

   * Targeting Linux containers:

     ```bash
     docker build . -f Dockerfile.linux --build-arg FEED_ACCESSTOKEN
     ```

   * Targeting Windows containers:

     ```bash
     docker build . -f Dockerfile.windows --build-arg FEED_ACCESSTOKEN
     ```

If the `docker build` command completes without errors, then you've been able to successfully build a project referencing a NuGet package from the Azure Artifacts feed. Apply the patterns used in this sample project to your own project.
