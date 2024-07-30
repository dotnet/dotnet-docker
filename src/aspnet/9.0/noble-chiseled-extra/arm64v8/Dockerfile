ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-preview.7.24379.2 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='40ba5b06a1fa7ec16a856d6f8a1394793fb7c0c1afe0be1c2f67fc1f91f84b031fd0bd6c983ed9da1877aff4f4f3a5ca995d3c9ed6992e114407395aa951ffeb' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-preview.7-noble-chiseled-extra-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.7.24379.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
