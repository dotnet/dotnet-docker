ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:9.0.0-preview.4-alpine3.19-arm64v8

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.4.24173.3

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='15f04a9c471c12aac6b3d5405e76b5b8c92690bfb6d79c46e44ad9ddff8883553e90419829ea80ecaee1101be34b942dba3e305191a40c1c310191bc1845377d' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
