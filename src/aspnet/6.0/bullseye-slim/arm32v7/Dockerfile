ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-rc.1.21451.36/aspnetcore-runtime-6.0.0-rc.1.21451.36-linux-arm.tar.gz \
    && aspnetcore_sha512='2954333981ffeea1e70f1abb8762cd9f89cc22e810fde089e4a7ddf6b626f91a35bf35c325ca3a9c27ab9c547c7af6d5b22701ee3c890c53739dd8c1196ace95' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.0-rc.1-bullseye-slim-arm32v7

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rc.1.21451.36 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
