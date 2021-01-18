ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0-alpine3.12-arm64v8

# Install .NET
ENV DOTNET_VERSION=6.0.0-alpha.1.21066.5

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='0e4f0dcf50647cb91f5a76de73a7a259d415a52bed9dfef2e8749b58776fa390aa84093e5f3f82466c1b06b04d3febe5dec4505264eaccc6629ac8e858b0d182' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
