ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:10.0.0-preview.5-alpine3.21-arm64v8 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.5.25267.102 \
    && wget -O aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='a91ef2e7ae77263e7a2341b1454d3168b6ecb1500f14c0b428cd74545584563fcc36d10afe0da8be3147c213ae43bf4bf7b1740885c3a406553f141fc96cd2e8' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.5-alpine3.21-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.5.25267.102

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
