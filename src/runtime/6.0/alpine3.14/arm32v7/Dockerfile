ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.0-preview.7-alpine3.14-arm32v7

# Install .NET
ENV DOTNET_VERSION=6.0.0-preview.7.21370.18

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm.tar.gz \
    && dotnet_sha512='d874755fa39c11eaccc37404abaf6ddc8a7013647ed681ca15a52179fdc90ee856927fa02a68444008f4bcdf9264f527d1774eadd0c1d643ab0d694bdeffd464' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
