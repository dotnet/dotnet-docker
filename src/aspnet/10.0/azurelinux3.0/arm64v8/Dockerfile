ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:10.0.0-preview.5-azurelinux3.0-arm64v8 AS installer

RUN tdnf install -y \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.5.25267.102 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='06c40c811a2ad5c930bc0577e2e6919119267356b1121a4c535b3651972a4c2ddfdbb32cf6765360e3abcc50a85071f91acc93926ffd432a7e219625b3a9788f' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.5-azurelinux3.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.5.25267.102

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
