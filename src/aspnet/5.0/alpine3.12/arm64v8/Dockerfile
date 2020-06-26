ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:5.0-alpine3.12-arm64v8

# Install ASP.NET Core
ENV ASPNET_VERSION 5.0.0-preview.7.20311.11

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='b987921a026ac5fa021af3a3fcf2e74b7570ade28e8a52a68d89701a6ef9c62ef0602c72b6520677e20894f4da32436732b75b4c8009328e84c57008804d32b9' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
