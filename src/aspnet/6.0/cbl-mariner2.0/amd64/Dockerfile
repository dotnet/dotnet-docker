ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.20-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.20

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='df1f1a74134511319e1ddffafa53c74b89dd7aa4de2c71413527050785e2e36953a9ef0d531cdbbf81c7a3cd5d082781cb94a43b0d19d5d8122a2e90bd21974b' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
