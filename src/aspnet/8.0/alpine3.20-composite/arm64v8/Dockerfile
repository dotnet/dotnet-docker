ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:8.0.7-alpine3.20-arm64v8

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # .NET Runtime version
    DOTNET_VERSION=8.0.7 \
    # ASP.NET Core version
    ASPNET_VERSION=8.0.7


# Install ASP.NET Composite Runtime
RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-composite-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='d6da2d256622ac7733ee3b21573f59411854c7ce702a459e84ab1d830f172cf07250d7b9ee14b898bb18e414658f25e90ab9b1bfd21c02d5f0d1610e4d4dd9b8' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
