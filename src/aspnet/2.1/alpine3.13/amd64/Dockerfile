ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:2.1-alpine3.13

# Install ASP.NET Core
ENV ASPNETCORE_VERSION=2.1.28

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='24965731887e6e3ee26b18124c9d29897beb33b1d3a58ad03564a09aca65a00843a7df230bdb5941686a62c6538a9e381bafea87feb605d3d43a146bc07f8f7d' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet \
    && rm aspnetcore.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
