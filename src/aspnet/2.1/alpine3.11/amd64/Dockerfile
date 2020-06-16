ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:2.1-alpine3.11

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 2.1.19

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='ad38186e0c34185981be134cb97cfc874d0459f781df6bac6270f892658a38cfc1a7c26cfc973e6a0b4d98f3d38cb46a0f1448da5f0d9081f5610ce408e677c2' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet \
    && rm aspnetcore.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
