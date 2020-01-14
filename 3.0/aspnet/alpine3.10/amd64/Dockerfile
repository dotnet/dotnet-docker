ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-alpine3.10

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.2

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='e20995330effefc569881d5b1585a58af9453b26d947080f96a29fca728f842e63bdc9817716c84cfa5884576684b9f36e38fdab2e98bee41d7b47f85d98aa0b' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
