ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:9.0.0-alpine3.20-amd64

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://builds-int.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='09e3709664f099b4116f8a2aac4b365247d11d0d19ecae262949de38fa9d41cc6c521a67e5b1ffecd63c610c1e9b41459bfb18f62b9d9d3b5176e3856e9ad35b' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
