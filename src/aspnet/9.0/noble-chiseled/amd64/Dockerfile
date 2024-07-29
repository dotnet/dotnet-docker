ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-preview.7.24375.8 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='7ae4371c5ccde9ad2c9d4871e2d2602296fe3ed5213a461206c64f4b386cb4e0e4e8144e037123b2800f36e8a60cde7f0f1cd43771bfc020b45dcd5598e93b73' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-preview.7-noble-chiseled-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.7.24375.8

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
