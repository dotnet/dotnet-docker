ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:10.0.0-preview.1-alpine3.21-arm64v8

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.1.25103.6

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='3bdaad9b16ff5fa13adb556c6236bc7c7faf8a8e317b13fa4f265bbc024ef79b1eb7e3d38ce775b8781853d0cfd3ed9a784898aabfd844c9208d60e4b9f51091' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
