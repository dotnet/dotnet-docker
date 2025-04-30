ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:10.0.0-preview.3-alpine3.21-arm32v7 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.4.25229.110 \
    && wget -O aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='84591821568352318b7888fd445a6bb6ef9aa4aefb8b93ea48146ab2c9c713800718a5d0c1410b61b14d2e15d2b4ee8ae4dd7976082a95a86adeecaa53f41c8c' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.3-alpine3.21-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.4.25229.110

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
