ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.26-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.26

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='c0613c1d4e30ac85a705654dfd079efb247bcddc2ecfb4d85bb91a1993f5c2dc9b89dafdb801a16a900dc61297264865f16c75b9c5dd1958342bc8b76125103f' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
