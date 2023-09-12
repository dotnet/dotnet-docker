ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.22-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.22

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='ca9b09b2d87f16ad81997344b432bb810eb28cc858d5c88601bf0e6ee535bbd287fa56c86d6775e5d5a4c4a8975e9fb68cfbf69250dbac89e5b52b8d70d03e55' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
