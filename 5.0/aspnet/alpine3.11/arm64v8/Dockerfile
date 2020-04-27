ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:5.0-alpine3.11-arm64v8

# Install ASP.NET Core
ENV ASPNET_VERSION 5.0.0-preview.4.20221.8

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='4256e4c4b4c7816adc2757a376c2589c8648a344676e31f76f5845ff6a909b3bbafe5706215c15eed31ad01327082d91a41f6c2905034c00c61a001c76df2456' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
