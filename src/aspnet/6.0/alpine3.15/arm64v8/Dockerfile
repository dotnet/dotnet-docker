ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.7-alpine3.15-arm64v8

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.7-servicing.22322.5

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='efac4d9f91a8f442f6223f77e09eafd4fec4d2daac395f968549ebba37285804c77617919c9cd3b900805e3a876d4d8aae2ade31140a6f278598aca4fd0c5995' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
