ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0.8-alpine3.13-arm64v8

# Install ASP.NET Core
ENV ASPNET_VERSION=5.0.8

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='3a0c9f76a1879b335bba7015884ae57830e11cc0ec23149b92ab08a3dc28cbfb307446f593741cd32f20452199e691ba0af9d5395d3ab7ff1b8ff6327743024a' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
