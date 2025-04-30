ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:10.0.0-preview.3-alpine3.21-arm64v8 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.4.25229.110 \
    && wget -O aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='c6d0d21b19127e680a56d128617de5b3f8545b9906daa99c64c193d44f998e4314dc58f1ce99d08d7a9d52f64e93b2158d4e05655f3ab546a99f0e366e2c2368' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.3-alpine3.21-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.4.25229.110

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
