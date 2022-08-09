ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/cbl-mariner/base/core:1.0 AS installer

RUN tdnf install -y \
        ca-certificates-microsoft \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=6.0.8 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='b74676ca0d2f47a95533739fd36977bb1552890a81820ee51b29b3d6514398f0952362417bbb31fad4bdf031803cb3e8d2aaf065dfb154a78a1b471a536d4abd' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.8-cbl-mariner1.0-distroless-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.8

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
