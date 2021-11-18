ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM cblmariner.azurecr.io/base/core:1.0 AS installer

RUN tdnf install -y ca-certificates-microsoft \
    && tdnf clean all

# Acquire ASP.NET Core
RUN aspnet_version='7.0.0-alpha.1.21567.15' \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnet_version/aspnetcore-runtime-$aspnet_version-linux-x64.tar.gz \
    && aspnetcore_sha512='89fa917fa391f96412ae7691749208d185cc26c9e3e5aadf23950a10de9dc9d8e1af0cd03a3640a2f7050d74925dab002e31b1b6e3bc7ac48a4afc5120db6574' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.0-alpha.1-cbl-mariner1.0-distroless-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=7.0.0-alpha.1.21567.15 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
