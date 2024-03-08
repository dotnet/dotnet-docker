ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.27-alpine3.19-arm64v8

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # .NET Runtime version
    DOTNET_VERSION=6.0.27 \
    DOTNET_RUNTIME_ID=linux-musl-arm64

# Install .NET Runtime
RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='3efad6050ec2bc3a2df40aaf8d16761be204ea866728d4b2c09e0ba4f4dc46225ba683c1c8a1ed446d2aed80e6d9c8eb46892fc6f28f2b2107bbc4d277de2d76' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
