ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.0-alpine3.14-arm64v8

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rtm.21514.23 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='a875b76c690bd0daab0eb22fdb10136dde6b51c4569178d6a3263a6cf9832f8b23d6d9ffbffc58780bf6e639c252149ecfa2549f702326a5048bb74ffcece9eb' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
