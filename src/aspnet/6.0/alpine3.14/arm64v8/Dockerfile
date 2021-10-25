ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.0-alpine3.14-arm64v8

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-rtm.21523.4/aspnetcore-runtime-6.0.0-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='5c9b9a5d7c3e958a2005731a02b670b92955675178a7a4784f9690f9d9c81827d42ab4c93fb42073c8c7b75f032fbca521d507680f34507de743dbf18d42ac91' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
