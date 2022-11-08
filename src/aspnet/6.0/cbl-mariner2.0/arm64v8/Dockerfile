ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.11-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.11

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='b1b33c92a068e1d629873f54355e9da3e18410c0e3452248b3a8f5fc074d8ace32616a7f703af48a274871545287f723ae971fba9a379cd39b1003f5b348b6e8' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
