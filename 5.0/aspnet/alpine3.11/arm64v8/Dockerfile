ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:5.0-alpine3.11-arm64v8

# Install ASP.NET Core
ENV ASPNET_VERSION 5.0.0-preview.3.20210.6

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='b72f359b15c3cb705318476c6d1552ebb75254f76e17d423d93b8f0630a3fd0e321fda61e230b063e0502f7726d346722cd71863131abec8370da6553cae2844' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
