ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:5.0-alpine3.12

# Install ASP.NET Core
ENV ASPNET_VERSION 5.0.0-preview.7.20311.11

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='d5cd9cc6be6723593f26e9ca466726791b7aa86e08d3e62819a9f61be270534e91e5390af7ebdb89d14513ec03c56180088de60cc9ae944091227429ae1c8cad' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
