ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:8.0.13-alpine3.20-amd64 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.13 \
    && wget -O aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='f4992c2cbc8019378af0c8463d36c978caa943e53d49522037b802f7476ec9d82a6f69dfb2178f82f619bcedd6620ff0c1aba024760864eb0f73ce70c04fd8f9' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.13-alpine3.20-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.13

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
