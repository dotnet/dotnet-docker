ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-bionic

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.1.0-preview1.19475.24

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='44ec86faa0f5399afd3ed7eb301a90f18d98b82cec4cf336e882c9193565bd29aa5986f0de4caa24972df42c4ce7b6e8bc7c0b143c86fcb4b2255b3c123210e7' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
