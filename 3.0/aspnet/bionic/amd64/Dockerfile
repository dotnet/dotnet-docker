ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-bionic

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview7.19324.10

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='f72e7f72e913b5c0d6171102e9a72b5c956765590587a5cf5825a71ef2460bc6c2739fd9bfa96e34f49e2f6119b40b233115bc53514663c63b840fb46e239ca8' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
