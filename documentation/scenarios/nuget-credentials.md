# Managing NuGet Credentials in Docker Scenarios

There are often cases when your Dockerfile needs to build a .NET project that make use of a NuGet package located in a private feed. How do you properly provide your Docker build access to that private feed in a secure manner? The options below provide recommended approaches you can take.

There are two aspects to be aware of when it comes to protecting credentials:

1. _Exposing credentials in an image that is published to a registry._ This is the most obvious scenario. You don't want consumers of your image to have access to data they should not have. The options listed below demonstrate how to prevent this from happening.
2. _Exposing secrets locally on the Docker host machine._ This isn't necessarily an issue; it depends on your build processes. Just be aware that while your published image may not be exposing sensitive data, there can be sensitive data stored on the Docker host machine that you may want to clean up depending on how the machine is managed in your build workflow. For example, when using Azure DevOps pipelines with a hosted agent, there's no need to clean up data because the virtual machine of the build agent is automatically deleted at the end of the build. But if you manage your own agent, you're in charge of cleaning any leftover state should it be necessary. Because some environments log command lines, it is a security best-practice to avoid putting secrets on the command line.  To avoid this with `docker build`, store the secret in an enviornment variable with the name you want the build argument to have.  Then, on the `docker build` command line, refer to [just the name of the environment variable](https://docs.docker.com/engine/reference/commandline/build/#set-build-time-variables---build-arg) without the `=` or the value.

## Use a multi-stage build to protect nuget.config passed by build context

By using multi-stage builds, you can use the build context to pass a set of files to the Docker build that are used just for building the application and avoid having all of those files end up in the final image. If your `nuget.config` file contains credentials and is already stored on the Docker host machine, then you may want to consider passing it via the build context for the Docker build (also see [Passing secrets by file with Docker BuildKit](#passing-secrets-by-file-with-buildkit) for a related pattern). *Be careful when storing credentials on disk. Make sure that the machine and file are properly secured.*

Even though using a multi-stage build is a good technique to help avoid exposing credentials in the final image, it should be known that the intermediate layers produced by the build can still expose those secrets locally on the Docker host machine.

Here's an example `nuget.config` file containing the credentials:

```XML
<?xml version="1.0" encoding="UTF-8"?>
<configuration>
  <packageSources>
    <add key="public" value="https://api.nuget.org/v3/index.json" />
    <add key="customfeed" value="https://mycustomfeedurl"  />
  </packageSources>

  <packageSourceCredentials>
    <customfeed>
      <add key="Username" value="<username>" />
      <add key="ClearTextPassword" value="<password>" />
    </customfeed>
  </packageSourceCredentials>
</configuration>
```

In the Dockerfile, the `nuget.config` file is copied via the build context and stored in the file system only for the `build` stage. In the `runtime` stage, only the binaries of the application are copied, not the `nuget.config` file so the credentials are not exposed in the final image.

```Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj .
COPY ./nuget.config .
RUN dotnet restore

# Copy and publish app and libraries
COPY . .
RUN dotnet publish -c Release -o out  --no-restore


FROM mcr.microsoft.com/dotnet/runtime:5.0 AS runtime
WORKDIR /app/
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "dotnetapp.dll"]
```

This Dockerfile would be built using this command:

```bash
docker build .
```

## Using environment variables in nuget.config

If you do not store your credentials in the nuget.config file and instead retrieve them in some manner through a build process, you can use this environment variable with multi-stage build pattern. This pattern involves using the [`ARG` instruction](https://docs.docker.com/engine/reference/builder/#arg) to pass in the NuGet feed credentials to a `docker build` command. Care must be taken here to ensure that the `ARG` instructions are only used for intermediate stages and never the final stage to avoid the leaking the credentials in the final image.

Even though using a multi-stage build is a good technique to help avoid exposing credentials in the final image, it should be known that the intermediate layers produced by the build can still expose those secrets locally on the Docker host machine.

For this pattern, we're going to rely on a `nuget.config` file that makes use of NuGet's support for [environment variables](https://docs.microsoft.com/en-us/nuget/reference/nuget-config-file#using-environment-variables):

```XML
<?xml version="1.0" encoding="UTF-8"?>
<configuration>
  <packageSources>
    <add key="public" value="https://api.nuget.org/v3/index.json" />
    <add key="customfeed" value="https://mycustomfeedurl"  />
  </packageSources>

  <packageSourceCredentials>
    <customfeed>
      <add key="Username" value="%Nuget_CustomFeedUserName%" />
      <add key="ClearTextPassword" value="%Nuget_CustomFeedPassword%" />
    </customfeed>
  </packageSourceCredentials>
</configuration>
```

When making use of the `ARG` instruction, the name of the argument is made available as an environment variable during the scope of the `docker build`. This aligns nicely with our use of environment variables in the `nuget.config` file.

In the Dockerfile below, there are two stages: build and runtime. The build stage is responsible for building the application project. It defines two `ARG` values which match the names of the environment variables used in the the `nuget.config` file. But the build stage is not the final stage of the Dockerfile so the `--build-arg` values that are passed to the `docker build` command do not get exposed in the resulting image. All the final stage is really doing is copying the built output of the application from the build stage since that's all that's needed to run the application.

```Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

ARG Nuget_CustomFeedUserName
ARG Nuget_CustomFeedPassword

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj .
COPY ./nuget.config .
RUN dotnet restore

# Copy and publish app and libraries
COPY . .
RUN dotnet publish -c Release -o out  --no-restore


FROM mcr.microsoft.com/dotnet/runtime:5.0 AS runtime
WORKDIR /app/
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "dotnetapp.dll"]
```

Before running `docker build`, first populate the `Nuget_CustomFeedUserName` and `Nuget_CustomFeedPassword` environment variables with appropriate secrets. Then, this Dockerfile would be built using this command:

```bash
docker build --build-arg Nuget_CustomFeedUserName --build-arg Nuget_CustomFeedPassword .
```

Passing the username and password values to the `docker build` command in this manner can be useful in automated scenarios when those values are stored as environment variables on the Docker host machine or can be retrieved from an external secrets storage location and passed to the `docker build` command.

## Using the Azure Artifact Credential Provider

NuGet has support for [custom credential providers](https://docs.microsoft.com/en-us/nuget/reference/extensibility/nuget-cross-platform-authentication-plugin) that provide a mechanism to authenticate to feeds both interactively and non-interactively. One such implementation of this is [Azure Artifact Credential Provider](https://github.com/Microsoft/artifacts-credprovider) which provides a mechanism to acquire credentials for restoring NuGet packages from Azure Artifacts feeds. In unattended build agent scenarios, like building Docker images, a `VSS_NUGET_EXTERNAL_FEED_ENDPOINTS` [environment variable](https://github.com/Microsoft/artifacts-credprovider#environment-variables) is used to supply an access token.

If you're using Azure Artifacts for your private NuGet feeds, this scenario is for you since it provides a compatible experience between interactive developer machines and automated scenarios like Docker image builds. This allows you to use the same `nuget.config` file in either scenario. In this pattern, we'll use the environment variables and multi-stage build along with Credential Provider to pass the credentials for a private NuGet feed.

Even though using a multi-stage build is a good technique to help avoid exposing credentials in the final image, it should be known that the intermediate layers produced by the build can still expose those secrets locally on the Docker host machine.

The `VSS_NUGET_EXTERNAL_FEED_ENDPOINTS` environment variable is a well-known variable that is meant to be an extension of the `nuget.config` file, allowing both to be used together where `VSS_NUGET_EXTERNAL_FEED_ENDPOINTS` simply supplies the access token. The `nuget.config` file used here does not define any `packageSourceCredentials` element for `customfeed`:

```XML
<?xml version="1.0" encoding="UTF-8"?>
<configuration>
  <packageSources>
    <add key="public" value="https://api.nuget.org/v3/index.json" />
    <add key="customfeed" value="https://fabrikam.pkgs.visualstudio.com/_packaging/MyGreatFeed/nuget/v3/index.json"  />
  </packageSources>
</configuration>
```

Instead, the credentials for `customfeed` are defined in the Dockerfile by making use of an `ARG` for the access token:

*Linux*

```Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

RUN curl -L https://raw.githubusercontent.com/Microsoft/artifacts-credprovider/master/helpers/installcredprovider.sh  | sh

# Copy csproj and restore as distinct layers
COPY *.csproj .
COPY ./nuget.config .
ARG FEED_ACCESSTOKEN
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS="{\"endpointCredentials\": [{\"endpoint\":\"https://fabrikam.pkgs.visualstudio.com/_packaging/MyGreatFeed/nuget/v3/index.json\", \"username\":\"docker\", \"password\":\"${FEED_ACCESSTOKEN}\"}]}"
RUN dotnet restore

# Copy and publish app and libraries
COPY . .
RUN dotnet publish -c Release -o out --no-restore


FROM mcr.microsoft.com/dotnet/runtime:5.0 AS runtime
WORKDIR /app/
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "dotnetapp.dll"]
```

*Windows*

```Dockerfile
# escape=`

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

SHELL ["pwsh", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Change user to workaround https://github.com/microsoft/artifacts-credprovider/issues/201
USER ContainerAdministrator
# Install the cred provider
RUN Invoke-WebRequest https://raw.githubusercontent.com/microsoft/artifacts-credprovider/master/helpers/installcredprovider.ps1 -OutFile installcredprovider.ps1; `
    .\installcredprovider.ps1; `
    del installcredprovider.ps1
USER ContainerUser

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj .
COPY nuget.config .
ARG FEED_ACCESSTOKEN
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS="{`"endpointCredentials`": [{`"endpoint`":`"https://fabrikam.pkgs.visualstudio.com/_packaging/MyGreatFeed/nuget/v3/index.json`", `"username`":`"docker`", `"password`":`"${FEED_ACCESSTOKEN}`"}]}"
RUN dotnet restore

# Copy and publish app and libraries
COPY . .
RUN dotnet publish -c Release -o out --no-restore


FROM mcr.microsoft.com/dotnet/runtime:5.0 AS runtime
WORKDIR /app/
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "dotnetapp.dll"]
```

_Note that a script is called to install the Credential Provider. When `dotnet restore` is run, the Credential Provider is invoked to resolve the credentials and it retrieves them from the `VSS_NUGET_EXTERNAL_FEED_ENDPOINTS` environment variable._

Before running `docker build`, first populate the `FEED_ACCESSTOKEN` environment variable with a [personal access token](https://docs.microsoft.com/azure/devops/organizations/accounts/use-personal-access-tokens-to-authenticate). Then, this Dockerfile would be built using this command:

```bash
docker build --build-arg FEED_ACCESSTOKEN .
```

Passing the access token to the `docker build` command in this manner can be useful in automated scenarios when that value is stored as an environment variable on the Docker host machine or can be retrieved from an external secrets storage location and passed to the `docker build` command.

### Credential Provider Troubleshooting

If you are having authentication issues, try adding a command to the Dockerfile to print out the value of the `VSS_NUGET_EXTERNAL_FEED_ENDPOINTS` environment variable. This will help you to ensure it is set and formatted correctly.

Example (Linux):

```Dockerfile
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS="{`"endpointCredentials`": [{`"endpoint`":`"https://fabrikam.pkgs.visualstudio.com/_packaging/MyGreatFeed/nuget/v3/index.json`", `"username`":`"docker`", `"password`":`"${FEED_ACCESSTOKEN}`"}]}"
RUN echo $VSS_NUGET_EXTERNAL_FEED_ENDPOINTS
```

Example (Windows PowerShell):

```Dockerfile
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS="{`"endpointCredentials`": [{`"endpoint`":`"https://fabrikam.pkgs.visualstudio.com/_packaging/MyGreatFeed/nuget/v3/index.json`", `"username`":`"docker`", `"password`":`"${FEED_ACCESSTOKEN}`"}]}"
RUN echo $Env:VSS_NUGET_EXTERNAL_FEED_ENDPOINTS
```

Example (Windows Cmd):

```Dockerfile
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS="{`"endpointCredentials`": [{`"endpoint`":`"https://fabrikam.pkgs.visualstudio.com/_packaging/MyGreatFeed/nuget/v3/index.json`", `"username`":`"docker`", `"password`":`"${FEED_ACCESSTOKEN}`"}]}"
RUN echo %VSS_NUGET_EXTERNAL_FEED_ENDPOINTS%
```

## Passing secrets by file with BuildKit

Docker's [BuildKit](https://github.com/moby/buildkit) integration provides some enhanced functionality for passing [secret information](https://docs.docker.com/develop/develop-images/build_enhancements/#new-docker-build-secret-information) when running `docker build`. This avoids the use of `ARG` values which could potentially be exposed in the resulting image unless you're disciplined in defining your multi-stage build. But it also requires that the secret data be stored in a file on the Docker host machine during the `docker build`. If your `nuget.config` file contains credentials and is already stored on the Docker host machine, then this may be a good option for you. *Be careful when storing credentials on disk. Make sure that the machine and file are properly secured.*

Let's use the following sample `nuget.config` file which is stored on the Docker host machine:

```XML
<?xml version="1.0" encoding="UTF-8"?>
<configuration>
  <packageSources>
    <add key="public" value="https://api.nuget.org/v3/index.json" />
    <add key="customfeed" value="https://mycustomfeedurl"  />
  </packageSources>

  <packageSourceCredentials>
    <customfeed>
      <add key="Username" value="<username>" />
      <add key="ClearTextPassword" value="<password>" />
    </customfeed>
  </packageSourceCredentials>
</configuration>
```

The Dockerfile references the `nugetconfig` secret with the `--mount=type=secret` syntax and access it at its default path of `/run/secrets/nugetconfig`:

```Dockerfile
# syntax = docker/dockerfile:1.0-experimental

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj .
RUN --mount=type=secret,id=nugetconfig \
    dotnet restore --configfile /run/secrets/nugetconfig

# Copy and publish app and libraries
COPY . .
RUN dotnet publish -c Release -o out --no-restore


FROM mcr.microsoft.com/dotnet/runtime:5.0 AS runtime
WORKDIR /app/
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "dotnetapp.dll"]
```

This Dockerfile would be built using this command:

```bash
DOCKER_BUILDKIT=1 docker build --secret id=nugetconfig,src=nuget.config .
```

## Other Options

There are a number of techniques that can be used to protect secrets in a Docker build. Doing a web search yields a good quantity of material out there. The options listed above are considered to be simple and straightforward but depending on your environment and tolerance for complexity, you may want to consider other options. You're encouraged to do your research and find an option that works best for you.

## General Troubleshooting

* The sample Dockerfiles provided above make the assumption that the application's projects are all based on .NET. If you're attempting to build a project that is based on a .NET Framework project that makes use of a packages.config file, you'll need to run `nuget restore` instead of `dotnet restore` for that project or solution.

* Often times, restoration of NuGet packages is the first point in a Dockerfile that attempts to make a network call. Firewalls and proxies can impact network connectivity. If you're encountering connection issues when attempting to restore NuGet packages, make sure that the container actually has network access by adding a command like `RUN ping www.microsoft.com`.
