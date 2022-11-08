ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/cbl-mariner/base/core:1.0 AS installer

RUN tdnf install -y \
        ca-certificates-microsoft \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=6.0.11 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='12a30719aacd5b3dd444d075c13966a4bb1dc149c36bcbc0e685730f08d1c75eb3929749b89a88569ddb48bd8104db84aaee2ee097ac3a5fe6fff60c9f09f741' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.11-cbl-mariner1.0-distroless-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.11

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
