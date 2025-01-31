ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:10.0.0-preview.1-alpine3.21-arm64v8

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.1.25079.4

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='dba6f625cec43e4c03f05f6f34fadaf1a1a8fc0f791315c71dc8c3a9b7fe7f85cde7803446818c0c6a117ade3c4c4319afaf9d2593b0efa94cfd64856ed4b632' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
