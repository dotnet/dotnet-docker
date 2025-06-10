ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:8.0.17-alpine3.21-amd64 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.17 \
    && wget -O aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='a3540f89c96853538ee114145ac9991f222be8915b80394191814fdaabef3abc235d605799fa375962c0909af03b3d0bf1ac760342c566140d914f7f706692be' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.17-alpine3.21-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.17

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
