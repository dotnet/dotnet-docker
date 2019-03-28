ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-stretch-slim-arm32v7

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview4-19172-03

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm.tar.gz \
    && aspnetcore_sha512='bc611234ef84c322ac9740acd0a72e79b829e6b30a487ef4f24df3e57d8466c08107010555bf0d707cac2c234cf68664cd31f378cf5e80a3dfd4c60bcf921472' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
