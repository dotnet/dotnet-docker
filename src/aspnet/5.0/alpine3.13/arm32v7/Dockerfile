ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0-alpine3.13-arm32v7

# Install ASP.NET Core
ENV ASPNET_VERSION=5.0.2

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='40765bcbe7e0cfa8d7096561daae6ed7d803629b1570d3a0b3d78183f9b7c7cc19f8822067e1e36e94b05681b9e7793f635de3fdc1c333db2b5135f6da54a912' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
