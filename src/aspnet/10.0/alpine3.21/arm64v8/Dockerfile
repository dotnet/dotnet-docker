ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:10.0.0-preview.3-alpine3.21-arm64v8 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.3.25172.1 \
    && wget -O aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='8decf0ecf5dab0c1d09832c3419c08cb6ba027fc1713f0a17ab7a61b89369867dafabaf1704340e0db81753b65aa63e131b75969fe7d7ee58e01c8854c6cba6b' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.3-alpine3.21-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.3.25172.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
