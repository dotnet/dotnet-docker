ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.7-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.7-servicing.22322.5

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='ee00fbdcf3eccfd9bf2f073dba43431c4f586b8823faa1be7350678c7dceb8ca3dbb7279241e9715bad364e2b5f42a4f4079c41d85f315730336c1fcbe30f294' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
