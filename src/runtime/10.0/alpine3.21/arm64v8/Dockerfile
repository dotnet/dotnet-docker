ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:10.0.0-preview.1-alpine3.21-arm64v8

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.1.25078.5

# Install .NET Runtime
RUN wget -O dotnet.tar.gz https://ci.dot.net/public/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='923311edc156b06fda63532c4f02235395d0c1d11a6e3552a71e95e8a68ff0474c54966e37bd375a4e16862fafa9fee87663e2e02b393bdeeddcd12f597a4e29' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
