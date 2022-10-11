ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.0-rc.2-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-rc.2.22476.2

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='7c0c28f581d2aa8be3a329eb938efea0a0922bc7d68daf0e7f8cc8ff299b6d5f3a138c9162df9f07067905c7737ed8a1a213b3e33d401b98efbe2eabfb80b757' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
