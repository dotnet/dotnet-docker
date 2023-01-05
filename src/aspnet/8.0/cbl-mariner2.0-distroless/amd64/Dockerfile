ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/cbl-mariner/base/core:2.0 AS installer

RUN tdnf install -y \
        ca-certificates-microsoft \
        gzip \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.0-alpha.1.23054.21 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='18f04aa2f733c0c94a422ea7aeb7b61f6bdca227c6b412c35ac243469338731307924765c8c6fc4505b637403462096a36f8a0319cb2999354f7504b2dbf3b8c' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.0-alpha.1-cbl-mariner2.0-distroless-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-alpha.1.23054.21

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
