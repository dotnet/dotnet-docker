ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.27-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.27

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='2b925b4ec480f58914b6104ae80d9bbd5ebd482c6cbaa66c9c94888087c4c19081ec43081a5a563659a784adfac8876d587f408b44fa4f1427623ac0238d9a12' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
