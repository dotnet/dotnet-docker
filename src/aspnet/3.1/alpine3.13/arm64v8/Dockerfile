ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.17-alpine3.13-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.17 \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='8cd21703bb4940323c7f7f6b7237163b2833ba5208357b2ce98e1eb9f43d91833094cfa550d27d85fd9c680dedb0769d5e690f457d5475867461ad3952521d3a' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
