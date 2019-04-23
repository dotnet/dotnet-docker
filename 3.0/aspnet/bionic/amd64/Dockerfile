ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-bionic

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview5-19218-09

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='0d2e9fbbc542e8f429c23adcf5f7ffe62b55e535d62a6c19e90c0d88868ff6531b2fb9ab9ab4c7f024727869c261bca63d04590d05934cbe029e3c5f6e4d4ea1' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
