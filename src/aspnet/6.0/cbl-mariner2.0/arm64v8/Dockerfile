ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.20-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.20

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='4443521228f92fee431a98c5476df40beac126736f710f6750c06a88eab79b98ba3f23ca882d1e5b3458257becb9a46a46fdf289fa82b9946cdcffeeaccdab6a' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
