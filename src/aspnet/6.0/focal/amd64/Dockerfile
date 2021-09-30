ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:focal-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-rc.2.21470.37/aspnetcore-runtime-6.0.0-rc.2.21470.37-linux-x64.tar.gz \
    && aspnetcore_sha512='0493c7f774f3e26d20e13f2c5fd5dc2fce45d197c7602fc3d92c6164bc2baae371a0f1e0bdc60f27d6dfa49c8c5867bc5a82ba3bc67bdd2257bdc0cb4aaccac1' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.0-rc.2-focal-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rc.2.21470.37 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
