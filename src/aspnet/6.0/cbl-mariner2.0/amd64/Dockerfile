ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.18-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.18

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='6c823cf81c4dca4d00242902068573fb335275d963d18bf5090f3bfb5cdfdacaeadf5f2c787d509f5c329c3bfdfe209d14303b87c1de13b3e9c4ec8914842989' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
