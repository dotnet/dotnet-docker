ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-disco

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview6-19272-02

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='61fffb0ac710af8d97c87ee0f888a94af1a027e3c937c48fa94671f98a0a8b024c5b25dd5bbecfa94c2b149cb4e32d5bd5b6d295169e6d95c2f7d9d1f1d5245a' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
