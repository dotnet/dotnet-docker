ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.32-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.32

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='fcad4bd1a8705bdba0f4c7d9b34d00df5e7e85ee500da46acef90b8e84c87a624c78564e9395cc9b7919b777b864ca15c30e0d0e61274402f4642e4b5c843e7a' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
