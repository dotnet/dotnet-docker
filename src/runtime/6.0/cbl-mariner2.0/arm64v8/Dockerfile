ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.6-cbl-mariner2.0-arm64v8

RUN tdnf install -y \
        ca-certificates-microsoft \
        gzip \
        tar \
    && tdnf clean all

# .NET Runtime version
ENV DOTNET_VERSION=6.0.6

# Install .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-arm64.tar.gz \
    && dotnet_sha512='659bf64c5f2e11f2b8cf01dc595a4280d1960bf484fc379d3a382660eea7adb6e69ace49d84522a85920ed53fa2ffb95a3b0ca0ebf63dc909b865028174ed29f' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
