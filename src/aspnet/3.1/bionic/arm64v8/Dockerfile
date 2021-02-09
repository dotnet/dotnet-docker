ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1-bionic-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.12 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='ad0f2bec8852037da08d8399ce200f5dde852453f0098b6c9b6451c1050fb7ff49a2fcedcf91f027af758782dfd5016b411d7c74bf8f3f1a19a93a129e48cb1a' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
