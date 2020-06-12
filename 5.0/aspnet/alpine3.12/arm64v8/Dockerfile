ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:5.0-alpine3.12-arm64v8

# Install ASP.NET Core
ENV ASPNET_VERSION 5.0.0-preview.6.20311.17

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='dd0caa495a2a28ed441e8b28688e8febd0f4e206f14d7720c1dad556f6ba4d57cb9d5f170dcf99e23ce1aaa4c7490119e83a14716d999a32fab9e1f1e5486115' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
