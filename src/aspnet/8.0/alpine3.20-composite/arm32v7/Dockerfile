ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:8.0.7-alpine3.20-arm32v7

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # .NET Runtime version
    DOTNET_VERSION=8.0.7 \
    # ASP.NET Core version
    ASPNET_VERSION=8.0.7


# Install ASP.NET Composite Runtime
RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-composite-$ASPNET_VERSION-linux-musl-arm.tar.gz \
    && dotnet_sha512='ba4cd631dc1f88cd8bc7fd8e5c3924758a81585d5f8d1e3ed97bf2efc8bb25d740d0a5d38bfcb67a9454c642f7b6d2b5b2e4faa2d4447687973c2c3584a365a4' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
