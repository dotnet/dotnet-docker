ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:9.0.2-alpine3.21-amd64 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.2 \
    && wget -O aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='ff070ebfabb1fa776df69ce6fb1e6990069ff6e4b795dd0ea0ee028dcceaa278500f695c9ba6b6c839310ee0d9aaca398e079ebd9081ec36c1e2d5a63c5bc109' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.2-alpine3.21-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
