ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.35-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.35

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='a20b3d6f4c60ab319f94054b0875fedd562718b0bc612a0177049a99fcb81d7d164f4281c6af2bc55b79439b6adb13fc522c89e370cebed5945a310cd4a191af' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
