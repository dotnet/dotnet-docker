ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:9.0.0-alpine3.20-arm64v8

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # .NET Runtime version
    DOTNET_VERSION=9.0.0-rtm.24516.10 \
    # ASP.NET Core version
    ASPNET_VERSION=9.0.0-rtm.24516.19


# Install ASP.NET Composite Runtime
RUN wget -O dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-composite-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='f9ba70f21fe32c430a87fd346ea51ba9b187ef2d3ad19cb7aa4a2286ac69ff18f4e87e1d60f246fb6ae020687e20e67b6355b0d60ec9ebc01aae038062bcc04c' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
