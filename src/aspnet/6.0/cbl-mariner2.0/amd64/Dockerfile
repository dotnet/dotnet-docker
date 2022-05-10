ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.5-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.5

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='086e9e190dbb26c78aee9050edcd6a3413709638bf9f5931771b90e23965e88c0a8c06d7618daa4f720afad3d41ee0efe8df025099b0ac46b2267c8647c4a2ab' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y aspnetcore.rpm \
    && rm aspnetcore.rpm
