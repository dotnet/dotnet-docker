ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:9.0.2-alpine3.20-arm32v7

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# .NET Runtime version
ENV DOTNET_VERSION=9.0.2

# Install .NET Runtime
RUN wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm.tar.gz \
    && dotnet_sha512='38e03d8c12fa4520e311cf2d15fb2c2f0e019c7165b13fcbe58fc46914743ecea8e0ac4d914385b1430cfb5e0df3db6299424cfdccea6ffe469a681e2d9d93b7' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
