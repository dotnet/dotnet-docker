ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM cblmariner2preview.azurecr.io/base/core:2.0 AS installer

RUN tdnf install -y ca-certificates-microsoft \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=7.0.0-preview.4.22224.3 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='268d7095520a06c635818676f484392649dcfb2c5c794cf0654e85bae22126538faf17132f445e2f5a078804004802f0479fdf7325f3ab225b7e18f984e08639' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.0-preview.4-cbl-mariner2.0-distroless-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-preview.4.22224.3

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
