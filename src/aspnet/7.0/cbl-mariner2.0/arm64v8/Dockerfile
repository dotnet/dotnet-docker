ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.0-rc.2-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-rc.2.22452.11

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='47b68e573f505fee2ff78dc2b1e4863d6beb86bbd884def280a5d35e7c0e64bfbd862e47c77b89cc1497e8320ec319db07078079d9d855b34bb2ee86ea34345a' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
