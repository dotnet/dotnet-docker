ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-disco-arm32v7

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview8.19377.1

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm.tar.gz \
    && aspnetcore_sha512='4008b1979c527533cf83e394741bd14b0ae6778037e616aef9203b18e656756195356ae9eb1c0837f44e7783fe3ec1f9e8d3d772f03b39f0f89238869ab5be84' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
