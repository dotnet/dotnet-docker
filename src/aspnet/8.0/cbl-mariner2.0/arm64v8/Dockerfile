ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:8.0.2-cbl-mariner2.0-arm64v8 AS installer

RUN tdnf install -y \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.2 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='9e5733a0d40705df17a1c96025783fd2544ad344ac98525f9d11947ea6ef632a23b0d2bf536314e4aeda8ae9c0f65b8f8feee184e1a1aabfda30059f59b1b9a6' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.2-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.2

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
