ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:10.0.0-alpha.1-alpine3.21-arm64v8

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-alpha.2.25070.3

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='1aecbb45c1dbbcff7ad8f79827719bab2f352467387ff34ab110cfad60f6912162560f0fc46f3e8742af1861a2b489755b21d37793cea236e7543793e4f12803' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
