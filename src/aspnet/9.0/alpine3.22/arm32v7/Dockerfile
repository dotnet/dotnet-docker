ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:9.0.5-alpine3.22-arm32v7 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.5 \
    && wget -O aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='62bca149538035454e52122e506572dd26ed13b3707ec77dcc0ad7ab39ed38d7693170676e38eebceb2529e8b56afa5878f1c0874f3b0d8391ec017d263867af' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.5-alpine3.22-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
