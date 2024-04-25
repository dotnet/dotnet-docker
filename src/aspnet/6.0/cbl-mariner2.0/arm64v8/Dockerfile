ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.29-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.29

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='a2c6f22d4dcc647fc0de728e274ad68a5a13ba2023503e776a20082590089a753efa84ddcf1c801c404953d3c1e2af45d85a5b8b95c2738fdae8f6286f9ba08d' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
