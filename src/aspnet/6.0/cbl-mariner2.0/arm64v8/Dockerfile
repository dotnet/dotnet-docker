ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.6-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.6

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='f5cd46d8f0ed09750893b8c828c9df872363c5d709f16d8be437304bdbd09f6127a43d57cbb01a66f0ac75771e7e29c4254302ef74dfc1b5267bc3296ceab8cb' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y aspnetcore.rpm \
    && rm aspnetcore.rpm
