ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.25-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.25

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='199c946c2b13682a4420d62081a54d0c249653a6035e5c8f88fad9888e278d1e6699d55e596ffb31ba4964d502c4f8a77d86f2430524e0ba8a8504106c9abccc' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
