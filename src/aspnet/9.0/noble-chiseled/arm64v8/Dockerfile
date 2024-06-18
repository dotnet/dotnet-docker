ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-preview.6.24309.2 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='429c62be9ad84f20991826fe0a2e35e5aebbccd36071ce3456d85e2e1143d729a45eb99088f02239ad1d42cb9ffe31042472f55fecfa90111a8c48a1e4b3cb7b' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-preview.6-noble-chiseled-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.6.24309.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
