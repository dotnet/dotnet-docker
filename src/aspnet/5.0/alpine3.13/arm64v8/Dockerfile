ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0-alpine3.13-arm64v8

# Install ASP.NET Core
ENV ASPNET_VERSION=5.0.7

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='eb976552dd3b6ea289648fb5c107521bf4ee6e53980fed6042207bb5ebc97fefe33f210e5213d4bbc4b1d415a8d8bc02d70e3ba4732c62c8241c0b35a9ec9a19' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
