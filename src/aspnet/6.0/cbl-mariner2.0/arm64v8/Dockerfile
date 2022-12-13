ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.12-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.12

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='74ca4d91f60afba8c37b561ef7f45db6d87efd2a6ca0a8431be10882c433529ac6a6c82080b6c38f62e96d5531297c021b0a5ffaf578d145aa3510c8063f8b7c' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
