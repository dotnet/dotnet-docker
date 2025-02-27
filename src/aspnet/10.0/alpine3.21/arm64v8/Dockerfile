ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:10.0.0-preview.2-alpine3.21-arm64v8 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.2.25126.5 \
    && wget -O aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='962f88991845f9a6e816dde0dff21566f2510241ad48aca3abf6896ad449e4153e5d02082917e6d13a209d5c553dd81a3a3acdca021e272bd2e8c581c34a7f06' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.2-alpine3.21-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.2.25126.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
