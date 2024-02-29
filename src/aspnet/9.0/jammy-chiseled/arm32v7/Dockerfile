ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-preview.3.24128.1 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='d4dafd571089131cb3645a6e6cd344ec7b5e1a7b5fe3290d4ef927852f095153da8734428b8d56328a209136d8b49fefe31dfe93e5eac448a6723a0544807a9c' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-preview.3-jammy-chiseled-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.3.24128.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
