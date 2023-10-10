ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.23-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.23

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='048a32d3c87092c262048a29a3e780ecdc97fa80d5769c84f8e651e68bc6d05343349fd6aa3700cf39aac1eca950e32d81199765cfd3a49d4f07b6ee8f00d04a' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
