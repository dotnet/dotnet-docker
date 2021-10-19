ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM cblmariner.azurecr.io/base/core:1.0 AS installer

RUN tdnf install -y ca-certificates-microsoft \
    && tdnf clean all

# Acquire ASP.NET Core
RUN aspnet_version='6.0.0-rtm.21518.18' \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnet_version/aspnetcore-runtime-$aspnet_version-linux-x64.tar.gz \
    && aspnetcore_sha512='080834742e72b49c42057616ef26c6bd957dd20343d9deedd83466e1d551aeb25c84bc0445f47985afc6aa0b8120d2637b82d73051d9401af33582f76ffe7b79' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.0-cbl-mariner1.0-distroless-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rtm.21518.18 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
