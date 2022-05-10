ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.5-cbl-mariner2.0-arm64v8

RUN tdnf install -y \
        ca-certificates-microsoft \
        gzip \
        tar \
    && tdnf clean all

# .NET Runtime version
ENV DOTNET_VERSION=6.0.5

# Install .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-arm64.tar.gz \
    && dotnet_sha512='bed49b74ad60d2701ddd638c61c215ad48f5c6eb88df140545d29901df60c6667474ca9d3bed6583dba605282ec64989ff2b431f098f0afc6ed155af59f7126d' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
