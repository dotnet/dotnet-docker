ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-preview.4.24223.1 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='40fe648686b21595a0a6531d8ae520d678b5d7e5c56847461a56283cc1ea127e2e1a09e568fd31b1caf30942db8a16260210c1c727b89f4a6d819791780540f1' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-preview.4-noble-chiseled-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.4.24223.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]