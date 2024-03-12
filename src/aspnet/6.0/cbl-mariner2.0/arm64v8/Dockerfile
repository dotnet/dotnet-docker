ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.28-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.28

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='26be6f7f973df56a8e0517d77401cf63f04a04ebfd594870e47e7f323c02da9c34464219be483e2a71d8975ecb0c2974c736715656122e3d31d08c3ab1679122' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
