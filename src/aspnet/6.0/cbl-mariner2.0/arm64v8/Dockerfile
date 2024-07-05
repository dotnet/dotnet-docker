ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.32-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.32

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='6bc5ea3d297b4e679b4827ea3a941c4dabf20564e0f142d02541a74d2aefdca4ca6d6c34da4f767a0115456a128dc205811eaf1f6ee961597da00914ce3ec203' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
