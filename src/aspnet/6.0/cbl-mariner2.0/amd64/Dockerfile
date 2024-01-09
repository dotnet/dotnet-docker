ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.26-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.26

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='49c2d57f24742c61c1a7f445cb0b151b5d7dbd49b26bbd46f1f4f561e6ec6ed588ef9accb3c7aba01de586e31550c8f2405633b9dc04f2fc0ffb7709363ec0c0' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
