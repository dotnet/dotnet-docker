ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:8.0.13-alpine3.21-arm32v7 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.13 \
    && wget -O aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='20a6322409e46f064f739d3948dcfa365879d3bfc1e704ec24f74accf9c8f464ac5782eac88cebf36d6732a3d45d6eff2bb4a89e39032332288de25e41065ba0' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.13-alpine3.21-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.13

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
