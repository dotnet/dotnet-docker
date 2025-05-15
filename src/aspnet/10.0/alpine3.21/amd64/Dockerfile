ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:10.0.0-preview.4-alpine3.21-amd64 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.4.25258.107 \
    && wget -O aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='881056acb894a52ee83d10c6ece60799728fefa20a63f85957c92c443be355c8a7db08663736c2aa23a7e6901bc65941e8e3913d9d2ca748c770376ab1b90b0c' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.4-alpine3.21-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.4.25258.107

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
