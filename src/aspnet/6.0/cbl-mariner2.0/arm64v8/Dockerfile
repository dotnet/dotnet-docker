ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.31-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.31

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='b3246c706ce6c25a52956fcc1f592ebc46424f4a1af4c2f303f729d29d9a20976ad9c2e2d2bc7fff569dd6168607fd02339414aeecf746b93e03307e6032b161' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
