ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.7-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.7

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='08e80a36dba464c8afc36c54987cc404e3b68813775098a2dcc1bdcad73a69bcee72b870d2a58ca35cf8915e5e32689af3d8019d0b2247f996c248d660cd896c' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
