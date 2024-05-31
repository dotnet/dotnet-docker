ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-preview.5.24280.2 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='23fab8ac2d5c77a39d6a92d45d5c623ea201f159bfc19c43c4e584f98298c820f86186761c8a55b2416ac2abe5cc6033a585b0873c1ea8ec3cbee579b765f386' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-preview.5-noble-chiseled-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.5.24280.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
