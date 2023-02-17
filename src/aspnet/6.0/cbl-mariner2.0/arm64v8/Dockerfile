ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.14-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.14

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='a4380553f0718f81a37eaf718acaadc34444f578b3fed2999664775fd6ba7a31d5a7f7e0bd1a6799594c42512d542e8cb97327cc71e6687e01980626ab06d210' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
