ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.30-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.30

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='33508182725369d39ca10544e99b966420abd1310bab23f9476fee8d2b7cbb6fb2f74b2f850584300c0d9f8e6b1cafc194286a23007744bf0211a146adec7e26' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
