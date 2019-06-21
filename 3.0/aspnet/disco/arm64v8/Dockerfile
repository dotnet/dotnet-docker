ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-disco-arm64v8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview7.19320.7

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='5dc812da8a68acce4cf915dc7cb4c66990b5b7e8419d67198ca84e77b7f36d984e3b88d8829933a7ebee82902a12d73a20fa11ef7e604311a22c8748440bb006' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
