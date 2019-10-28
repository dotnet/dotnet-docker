ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-buster-slim-arm32v7

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.1.0-preview2.19525.8

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm.tar.gz \
    && aspnetcore_sha512='59563bdd9741eaf89adf4a8215b74b4de573bc81a2a2ffa30c68bfdfb7da522e785e2722fc5647eace802f4d7413b1fce57c046f40a9d46b098e3303bca3cd02' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
