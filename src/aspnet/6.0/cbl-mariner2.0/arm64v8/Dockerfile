ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.21-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.21

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='d477356fef97bc5a6a2e18cc9ed2a83a87ada4f346e005eadea338f2a5f251835d7f15d040e607e9c4aa21a9c97fa27d2abbc6d65de1383abacb2e0e0e817041' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
