ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-alpine3.10-arm64v8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview9.19422.5

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='3bfd18b445f3f61531714c38f85994fe3e4584bc45212b3e53d65bb699f75edbd75b1441c5e40f107a709844760fea2bb05e06e6e40c66047d1e510b3870a3f2' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
