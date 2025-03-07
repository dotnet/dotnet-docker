ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:9.0.3-alpine3.20-amd64 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.3 \
    && wget -O aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='13585a919350ba2257f00a90ac2de0306b1e952ebba0c7f9039d7ec8da13554b2e3c86add01db83de4e647009b8a3ac66c3a68629e7701b463703b79db86e4ac' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.3-alpine3.20-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.3

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
