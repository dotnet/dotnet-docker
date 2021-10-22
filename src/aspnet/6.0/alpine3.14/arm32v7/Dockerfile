ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.0-alpine3.14-arm32v7

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rtm.21521.10 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-6.0.0-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='f1b5e40ef3d15c4a87cc4dd7e8d7957c37d25e00ec98232110139c6ac53060fed9e4c33107044e0abdb31980dc41acee1badbc3c8cbcc23a189353b6c90b3503' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
