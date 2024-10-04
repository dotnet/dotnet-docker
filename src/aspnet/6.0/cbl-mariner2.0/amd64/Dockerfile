ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.35-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.35

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='eec7fa63ca4d0f62251ac62b636dfa5fe4d511577da65551a039a397087b867274b37952195cd7b72d998f79589e01c69fcf6ddbba944b2e87504197630e22ba' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
