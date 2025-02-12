ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:9.0.2-alpine3.21-arm64v8

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# .NET Runtime version
ENV DOTNET_VERSION=9.0.2

# Install .NET Runtime
RUN wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='0bb0dc7a4388c5b95d4fec9fe7ca1273f9afa502021c73eb946ed2928d6d6d0836414400f667eff30da97b83b04f66d3032c2344bb2587007b4001d75b696a16' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
