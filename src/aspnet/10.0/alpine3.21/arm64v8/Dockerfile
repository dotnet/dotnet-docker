ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:10.0.0-preview.3-alpine3.21-arm64v8 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.4.25227.102 \
    && wget -O aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='db6d8c4f8c035703c0bcb42fd442a306ca76523147675dcd541d33ef265c2d335418ae3cc7ea0e39939d83483622709f33dbd51ef5c01feeaf3a5400a1a30dc0' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.3-alpine3.21-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.4.25227.102

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
