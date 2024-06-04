ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-preview.5.24303.2 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='42332d4f975d2adbd5c4433cffdab7738ca966e2faa2e0a72f0a862f9ebd29fc62c21f84b2558a5c240698a3872d8be7a9073af61c06c5af9ae501414b58ed1c' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-preview.5-noble-chiseled-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.5.24303.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
