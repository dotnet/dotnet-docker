ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:10.0.0-preview.1-alpine3.21-amd64

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.1.25080.5

# Install .NET Runtime
RUN wget -O dotnet.tar.gz https://ci.dot.net/public/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='93638a5e766b5859ae58ec3ba5dd81a60a0e6e2e4265d09b1bd71e42987216def1e94aa1c398e33d20baf6b7c021818cee111173b5771ff0ce2bcf56cc964c01' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
