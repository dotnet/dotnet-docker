ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:8.0.11-alpine3.20-arm64v8

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # .NET Runtime version
    DOTNET_VERSION=8.0.11 \
    # ASP.NET Core version
    ASPNET_VERSION=8.0.11


# Install ASP.NET Composite Runtime
RUN wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-composite-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='051b1087f6fc052f9c52c4f00d526c52c6c16705eae81ca5a5f436503d17eadb668a2065eba8c70214b3088d3a3d3a68071946c5ffb1f922b8ba5a43375158d3' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
