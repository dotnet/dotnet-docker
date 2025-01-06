ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.36-alpine3.20-arm32v7

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# .NET Runtime version
ENV DOTNET_VERSION=6.0.36

# Install .NET Runtime
RUN wget -O dotnet.tar.gz https://builds-int.dotnet.microsoft.com/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm.tar.gz \
    && dotnet_sha512='3dd539816405ec2d818f611a7cb98fe4069960e8ac4664ccca04ff8b2e0679387b0a3f2cc5045bef95657626ac1045bedc4e502da304ee54678afbbe83dcc3b3' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
