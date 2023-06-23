ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:8.0.0-preview.6-cbl-mariner2.0-arm64v8 AS installer

RUN tdnf install -y \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.0-preview.6.23322.7 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='eb3bf00fca510a8b02a96078e30787ef92bc3b70792a8bbf04438a22d7696fb1561f7718d0e54668212cd6b0c46c253b9729fc738642e19790930d19a91d00cc' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.0-preview.6-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-preview.6.23322.7

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
