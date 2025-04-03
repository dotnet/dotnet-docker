ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:8.0.15-alpine3.20-arm64v8 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.15 \
    && wget -O aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='025b2cc23591a1475755fc3b321e7d05aacda34c5616170eb47e4842bd978a4636d47d87c0666dadf7592ffd82db59427bcef953a0a5d1842d8c321d01c01e6e' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.15-alpine3.20-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.15

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
