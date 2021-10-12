ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0.11-alpine3.13-arm64v8

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# Install ASP.NET Core
ENV ASPNET_VERSION=5.0.11

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='93e7db6f87eb7068726330567f277cc930ec3ff32d04b8ec3dd19167a58524d067dc78ef01614c0ba0d3421a2dd23bd497fdf01615f4a3884d5593298965eab2' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
