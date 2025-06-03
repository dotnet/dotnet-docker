ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:10.0.0-preview.5-alpine3.22-amd64 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.5.25280.105 \
    && wget -O aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='abf330f00dbe6c31d3e6d615beb2dbc6f87afd56ed492752d3273f49d5cdbb827961c41d3f670acd2b1cf1604b2e5ddeebe428ce20a3316154c77add70522732' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.5-alpine3.22-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.5.25280.105

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
