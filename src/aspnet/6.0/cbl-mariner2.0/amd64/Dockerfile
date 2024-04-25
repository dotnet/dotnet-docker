ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.29-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.29

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='9f899f291d0f336b93713920552a0079577e448f8167ab88b083a4b6365686a8e78f8502b4364748ebc3c1df42eb31b080722f52ff0f1c8fd53b4436cb956053' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
