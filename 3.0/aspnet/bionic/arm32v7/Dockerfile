ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-bionic-arm32v7

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.3

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm.tar.gz \
    && aspnetcore_sha512='f63f29877a02f9467b4016f6802315d7c9c29fbcededca458b50670af4bcb8cc16ce08568a82371ddf7dc67fb17f9f399888a15d9a5d608bf6102c551531c3fb' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
