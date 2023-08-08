ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.21-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.21

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='972e8241be1d6b5c4f647ffae92a2e8a5ab207e28184819dd813c4b87bab6bb34e669bd7f11be55e01f2b5788c14924f500744dde4112900b8b3910124eec990' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
