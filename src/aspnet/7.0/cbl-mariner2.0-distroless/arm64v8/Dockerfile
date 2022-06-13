ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/cbl-mariner/base/core:2.0 AS installer

RUN tdnf install -y \
        ca-certificates-microsoft \
        gzip \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=7.0.0-preview.5.22303.8 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='56db58200270d802e0f3125d3ac055dc2d9b7f33879ddc995920d014434fc33d92a39ed0d32b01592e03bc4ee4a0c684621e1b6c64a6d13253896cca41e48262' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.0-preview.5-cbl-mariner2.0-distroless-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-preview.5.22303.8

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
