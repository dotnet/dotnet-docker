ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM cblmariner.azurecr.io/base/core:2.0 AS installer

RUN tdnf install -y \
        ca-certificates-microsoft \
        gzip \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=6.0.4 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='eaff93db0a4cc0adc2fc54de5e9a6e4b0844398451c06bcf6b2867471b8ed4fd0528ad04fe7150aa5ed306d5e08a5e4219c6029b96da03ad2d1c58e7a0ddacaf' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.4-cbl-mariner2.0-distroless-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.4 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
