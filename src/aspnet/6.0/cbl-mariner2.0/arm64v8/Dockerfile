ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.36-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.36

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='285e2b98c9022c8067bad0deaae868e84965aa0cae02a46c0cbb2eec1f7c0a39f860f2c2e5ee6ac8e37c1bf7769b972a7e676d44e55459aff0a7b061a9ee928a' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
