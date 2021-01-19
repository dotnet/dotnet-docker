ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-alpine3.12-amd64

# Install ASP.NET Core
ENV ASPNET_VERSION=6.0.0-alpha.1.21068.3

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='0ff903bb769b26ab0b2b6905b8f7e12521d590fad046cbdd36d42c96d8b81127e5a6c166845eef95d3fab69740f4989458dddfb6d3ed225b4afdfb96442aac20' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
