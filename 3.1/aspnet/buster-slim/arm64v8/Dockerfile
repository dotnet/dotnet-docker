ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-buster-slim-arm64v8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.1.0-preview2.19528.8

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='1709ca4d3edc1f5330886c24046c92746ad270acd7fd24674cba08052d3c6080396d0a13eb5535a1e33773ea3497b40fcb29dce24b506dae592d0111b6f0e0a6' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
