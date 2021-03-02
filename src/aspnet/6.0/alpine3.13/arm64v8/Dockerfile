ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-alpine3.13-arm64v8

# Install ASP.NET Core
ENV ASPNET_VERSION=6.0.0-preview.2.21125.25

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='872aed4c5a043e78de0c8b0d39f60ea258df01ab078f7dc232cd641c31de83da43fef33d9096a25adc0a4745cc067d6a23aa737c40a2d9c9e67972b6ea779c79' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
