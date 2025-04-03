ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:9.0.4-alpine3.20-arm64v8 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.4 \
    && wget -O aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='67fe12c34c4859dd8c50b57145e2fefbb929dbe2a729a5a8bfb1a53441689f68295cff4d93194ff3b9611212f13674c70e142b2cfe3bf6a57710fd4206722898' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.4-alpine3.20-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.4

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
