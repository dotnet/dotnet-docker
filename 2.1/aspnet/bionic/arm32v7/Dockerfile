ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:2.1-bionic-arm32v7

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
    && rm -rf /var/lib/apt/lists/*

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 2.1.11

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm.tar.gz \
    && aspnetcore_sha512='5f4b016947e5706b6ed7a5bb9f767f1f966cac2267815c3f4e16e1623888b6731122d3779f46556038e6951e72c48b0888a9385c103d2ff6147efdff0d148064' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet \
    && rm aspnetcore.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
