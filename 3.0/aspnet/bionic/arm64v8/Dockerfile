ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-bionic-arm64v8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview6-19274-01

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='b5dc30d0b54a63020d804878cc7f2ee26336d84cfdaaf28f8cc1afd13fec00d09f3a10c75a6f2aa376e89b10d2311da64f5f7e5ee244cfa7871ccd5204ac0fd4' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
