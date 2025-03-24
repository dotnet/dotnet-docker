ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:10.0.0-preview.3-azurelinux3.0-arm64v8 AS installer

RUN tdnf install -y \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.3.25172.1 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='48568aa8a3fd431a6775c90f26754136c87b89dea9e07b304101bd8c6dc4961f1436fa5a0316f7e9535198a978636caf7425f9b7aae003b53a95a1e14a01b450' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.3-azurelinux3.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.3.25172.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
