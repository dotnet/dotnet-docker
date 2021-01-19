ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-alpine3.12-arm64v8

# Install ASP.NET Core
ENV ASPNET_VERSION=6.0.0-alpha.1.21068.3

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='a1b13d0bc88d1aba7dfe7104eefd528690fd73a9ff1150b07daada4d3541ab638541d02db9f674190f9f443ad3f9419be2108774ad2a7e910ad0d43adcac6cdc' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
