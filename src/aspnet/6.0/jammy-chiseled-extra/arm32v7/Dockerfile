ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=6.0.33 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='7fa8cce31c9aa0c83dfd167b7a116c8b4a34d21b1154efc781bc2a38c14d350e78b5b9ee6fb8e3814e64dec6c92e0f2bf5c5cc9af90c73ea38ccb66540604176' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.33-jammy-chiseled-extra-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.33

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
