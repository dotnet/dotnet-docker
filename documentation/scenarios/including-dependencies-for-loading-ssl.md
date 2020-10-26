# Including dependencies for loading SSL with ASP.NET Core in a Docker Windows Container

We may want to try using SSL certificates (.pfx with password) with ASP.NET core Docker Containers.

In some cases, we may need to make sure that we include the appropriate assemblies in our application.

Setting the parameter /p:PublishTrimmed=true may remove dependencies https://docs.microsoft.com/en-us/dotnet/core/deploying/trim-self-contained, including  one that help with loading SSL certificates.  There's an [issue](https://github.com/dotnet/sdk/issues/14238) with additional details.

## Checking the Dockerfile

For instance, let's check the [sample docker file](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile.nanoserver-x64-slim) which should look like the following:

```Dockerfile
# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY aspnetapp/*.csproj ./aspnetapp/
RUN dotnet restore -r win-x64

# copy everything else and build app
COPY aspnetapp/. ./aspnetapp/
WORKDIR /source/aspnetapp

RUN dotnet publish -c release -o /app -r win-x64 --self-contained true --no-restore /p:PublishTrimmed=true /p:PublishReadyToRun=true

# final stage/image
# Uses the 2009 release; 2004, 1909, and 1809 are other choices
FROM mcr.microsoft.com/windows/nanoserver:2009 AS runtime
WORKDIR /app
COPY --from=build /app ./

# Configure web servers to bind to port 80 when present
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["aspnetapp"]
```

In this case, we can see that we're using the `/p:PublishedTrimmed=true`, which may remove dependencies that we need for asp.net core to load SSL certficates.  We'll need to update the associated project file to make sure that the dependencies are included.

> The image `mcr.microsoft/com/windows/nanoserver` is a base image which is a subset of Windows Server Core.  Other samples may use `mcr.microsoft.com/dotnet/aspnet`, which may include additional dependencies from the ASP.NET runtime.

## Updating the .csproj file

We can update the [sample aspnetapp]([sample docker file](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/aspnetapp/aspnetapp.csproj)) to include the dependencies.

These libraries should be included for ASP.Net Core 3.1:
```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>...</PropertyGroup>
  <!--Include the following for .aspnetcore 3.1-->
  <ItemGroup>
    <TrimmerRootAssembly Include="System.Net" />
    <TrimmerRootAssembly Include="System.Net.Security" />
    <TrimmerRootAssembly Include="System.Security" />
  </ItemGroup>
  ...
</Project>
```

These libraries should be included for .Net 5.0:
```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
 <PropertyGroup>...</PropertyGroup>
 <!--Include the following for .net 5-->
 <ItemGroup>
    <TrimmerRootAssembly Include="System.Net.Security" />
    <TrimmerRootAssembly Include="System.Security" />
  </ItemGroup>
  ...
</Project>
```

### Building and Testing

With these changes in place, we can re-build the container.  Be sure that we're in Windows Containers mode for this build:

```sh
docker build -f Dockerfile.nanoserver-x64-slim -t aspnetapp:my-nanoserver-x64-slim .
```

Assuming that we've already obtained a certificate (.pfx file) with a password, we can place the certificate in a folder for mounting in the container (e.g. in `c:\https`).  Then, we can attempt to run the sample with the following:

```sh
docker run -it  -p 8000:80 -p 8001:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=8001 -e ASPNETCORE_Kestrel__Certificates__Default__Password="password" -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/contoso.com.pfx -v c:\https:c:\https aspnetapp:my-nanoserver-x64-slim
```

If we have trusted the certificate and we're able to update host settings, we should be able to attempt the https request in a browser.