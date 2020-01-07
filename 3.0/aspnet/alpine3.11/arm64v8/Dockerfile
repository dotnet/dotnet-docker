ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-alpine3.11-arm64v8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.1

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='716103e12b7d7c2b623fcd82a990f3772bede2fb0500fae9fb83de935f43d3b06e612f2dbe0eacde7abee8f49a24454e58e50537ad5d0977abe523f38a8d1e23' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
