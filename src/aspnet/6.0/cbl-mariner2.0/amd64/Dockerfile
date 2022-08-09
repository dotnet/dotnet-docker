ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.8-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.8

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='9cfb91e5a4221755fb152b5ea5d69a6e0f8b693b45656f23df17cabd4a5aeef321d893dadba61fd41821140544275d5789545a172198149e228ef1b64751f386' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
