ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.0-preview.5.23273.2 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='b924a737c3e6e69489a0c8b3de9e40e88d78f73d607b2a742efc7bf3cba6b71590247c95a47402552b2d64f5bfaa4b5874017c9eae8f623fe9d3108d618ce854' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.0-preview.5-jammy-chiseled-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-preview.5.23273.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
