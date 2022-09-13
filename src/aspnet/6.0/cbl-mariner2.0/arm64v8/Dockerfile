ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.9-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.9

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='f5c75fa7c24a9b5fac84f7d9cd3aa2adfdde15f9f22a56d29b7e09da2b5dbe3cfa6d7c37815fce556b8edd9b17f3f49d0281428039e56d34b624cf7dbca21175' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
