ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.22-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.22

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='bc978b7b972ddc3ee7b409cb3cc299d62d4b4a8b901492e6f1b1eddff8624695c70712e1465d369fe17e20c641cca5633f9b78fca6076efd41f737f71ba66160' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
