ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.33-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.33

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='bebee04f4a8546af9cfd71e3835af10b611a57bcd3011f412ca02235cdd95fdb185a9abb43d54726107329ba48f515c61cbf0c9bffe7830fd85b9cbd355987bd' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
