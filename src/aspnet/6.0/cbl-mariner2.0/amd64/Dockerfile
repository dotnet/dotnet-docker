ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.28-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.28

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='fe082b41a1dc0beff88b99d188c48c294bbdf5001150c5f5a1fec61703487a937bfa2910639b86f3913cace609813526125d79950b38100571328cd19f6f15af' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
