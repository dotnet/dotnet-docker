ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-alpine3.13-amd64

# Install ASP.NET Core
ENV ASPNET_VERSION=6.0.0-preview.4.21207.16

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='137c96363e039c03fdd36e82db53c4cc72acddeec219f8d5bb928ac017d2a67bb6e24e07b20d9aeed37b0a603006d662d616ae67507e9d21b2585e54d0adf493' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
