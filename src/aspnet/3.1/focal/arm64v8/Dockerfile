ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.21-focal-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.21 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='f3d014431064362c29361e3d3b33b7aaaffe46e22f324cd42ba6fc6a6d5b712153e9ec82f10cf1bee416360a68fb4520dc9c0b0a8860316c4c9fce75f1adae80' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
