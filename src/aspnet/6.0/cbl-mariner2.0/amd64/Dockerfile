ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.6-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.6

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='17944895ae7b4ff5e713da183947d88cf581173f824e534832874553d3067a3cfc17616cd9276dda119efd3c93c4936078b23c0559c53239e90c9a8d0907a482' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y aspnetcore.rpm \
    && rm aspnetcore.rpm
