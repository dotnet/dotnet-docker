ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0-alpine3.13-arm64v8

# Install ASP.NET Core
ENV ASPNET_VERSION=5.0.5

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='b46071e8b9e7b71d0ca57dbe524ab152ca1e3b5f75d443dff7dcdb80f61d5dee5044b8bde2a20e94d3d7f26aaf82a94bb0af9f5883446a929da3278eb55ffa27' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
