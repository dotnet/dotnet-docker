ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1-alpine3.12-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.13 \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='f81e3f87d47e2b51f7eb8a23262f96f92753a3f7ba023b9b4578c3f772690982d80e48cb670ddb27241ae46f3bdfba4d93336017590a6cf9027187f694032d5e' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
