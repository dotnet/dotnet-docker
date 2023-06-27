ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.19-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.19

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='ab806f9e91991c633d193c9924628a64ac988cc204475aa6cbfa1d21d4af4d430ed0c2b700f3c0066f6beefccf127249ad723dcfaece7b14edb27db6c2506a08' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
