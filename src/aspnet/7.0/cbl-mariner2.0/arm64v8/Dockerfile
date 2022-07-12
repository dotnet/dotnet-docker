ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.0-preview.6-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-preview.6.22330.3

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='e826f0e33c9a3aa48ac9df7d8ffa036e87270a777a5828433933671528e080ebd967a93c85a4e466d9ae66f26983cc4d52a8cbc1fa3c3a9584e6e7db8c1740ff' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
