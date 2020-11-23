ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-alpine3.12-arm64v8

# Install ASP.NET Core
ENV ASPNET_VERSION 6.0.0-alpha.1.20629.3

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='92ccd5f30eab542aeb08f736ff5a3b50ec88fe4ee4f164932620fcea4dbc498ece2617d0d80a7ae777bc857e8c313ca27e9e7be942ccafe9bf704faf0cf2d36d' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
