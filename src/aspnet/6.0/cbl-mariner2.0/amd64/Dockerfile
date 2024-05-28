ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.30-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.30

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='887437e297cac90faa80647628f131c65a1b2ca0fd01b17f2403da7c168d230984fa55d547c4f85f636c03cac4c580806850250c93477715929c08a9c6ba51bf' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
