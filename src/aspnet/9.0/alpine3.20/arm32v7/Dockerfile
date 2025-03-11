ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:9.0.3-alpine3.20-arm32v7 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.3 \
    && wget -O aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='416bf072c817abca07a3ec5e3527d8baf01f798c208a9091599a4d1826f3e9adb6f0b40bbbec597548e0715283cc166f5e9ff61836426c2de5c726b45f5fc8be' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.3-alpine3.20-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.3

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
