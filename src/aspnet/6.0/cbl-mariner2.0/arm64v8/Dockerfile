ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.13-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.13

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='83c5cee13c67bd25928bae801c9b97c3eb79ca53e744e5b4abc539e8a6b23c97a9ac181dca307ebc797a45dcefb9a8f345633636b11b1e7730deaf09091274b8' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
