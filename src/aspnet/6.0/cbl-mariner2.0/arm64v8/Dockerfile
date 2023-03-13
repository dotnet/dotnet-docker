ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.15-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.15

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='8023e20c1018c6b7dfd2ef740ba239164a748f715dab4d8ad3ce3bb35d881e13f009f98a802fbef87099a6876eeae9ce233d835fc5ec2ff7b9ed267cfd56a29f' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
