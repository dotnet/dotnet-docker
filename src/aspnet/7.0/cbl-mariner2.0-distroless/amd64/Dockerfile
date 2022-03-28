ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM cblmariner2preview.azurecr.io/base/core:2.0 AS installer

RUN tdnf install -y ca-certificates-microsoft \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=7.0.0-preview.3.22174.3 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='0d681a3a157b47e2921ef68f6b8fdd439e821b2f9cdcf2a81f626163eb5a442ec27102a60447a4b535753cd6a4a7374cf9e01c2a769cc5933acba17f70a2339b' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.0-preview.3-cbl-mariner2.0-distroless-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-preview.3.22174.3

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
