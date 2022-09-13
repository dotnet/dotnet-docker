ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.9-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.9

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='e1e3ea08570876f35c7cc0ffa657bae3271c19afed6917241e260385b1ab29d62df0777685b924a1bd31210da75fcd82118b9a94afd6c39399d1c6642dafb543' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
