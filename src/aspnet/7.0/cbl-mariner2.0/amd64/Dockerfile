ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.0-preview.5-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-preview.5.22303.8

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='027b9d0fb59234850945b7b18f9ac837adcc7525a3f8bedbb0c63f83ed7db54293e948c8ecd4aebaa1ce0880e5c99695ed8815a7927ed92be0d60e7a776a30e6' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y aspnetcore.rpm \
    && rm aspnetcore.rpm
