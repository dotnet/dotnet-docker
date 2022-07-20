ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.7-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.7-servicing.22322.5

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='1841700c61a767e42675d777052af213b1dc8161c54eba764ef23bcf5767ed21a1f24793c76793ec13c7d0d6e7e6585eb61dcbfd8f99a280616964cece89e7ef' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
