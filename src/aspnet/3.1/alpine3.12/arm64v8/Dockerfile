ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-alpine3.12-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.6 \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='0b1a5e4817f087f6538c24ecbfd962681a8c04a944890145b768d894fb6f0b657a30842b7b2e01f5d39be39e4c8e86a07950f0ca8fdf8729699a16a526931cc4' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
