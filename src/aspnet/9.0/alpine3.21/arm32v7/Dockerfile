ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:9.0.6-alpine3.21-arm32v7 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.6 \
    && wget -O aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='f0e7eb3a1782efd88a60c5c45b89f244068fdc9cfb19e0ffd0b4760a0f7125f43f2bd00eaaf69ad5ed2b5f2b74d2847c916ff79579c77628a7d35865d72baac3' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.6-alpine3.21-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
