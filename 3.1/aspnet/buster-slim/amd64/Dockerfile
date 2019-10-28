ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-buster-slim

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.1.0-preview2.19525.8

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='860fa930af029c61c48fb0e09290148e9ddcce4f35fd6996d922d12d9a4ab0f703581c85dd564ce4ca113688f08dfec35ab18d381c813cbbb4e95a977410e4e8' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
