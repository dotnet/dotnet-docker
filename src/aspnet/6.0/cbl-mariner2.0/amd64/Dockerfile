ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.24-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.24

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='fc7e136dba283603f0e2bd842b8bc1348c739b66e3be13fbf7c41dd46699cd658f0f36fd5ef0b39ca72f5fae817c9661cde8e5ba4a4af81381588a0351f002ee' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
