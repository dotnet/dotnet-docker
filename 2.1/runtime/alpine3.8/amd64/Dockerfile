ARG REPO=microsoft/dotnet
FROM $REPO:2.1-runtime-deps-alpine3.8

# Install .NET Core
ENV DOTNET_VERSION 2.1.7

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='e362a0497fcd524ea1d2fa512bf439e54ca487660a3f3e3e1b5b78b98efb4c5bfe0c4a092501adcc02ade2436ae656b9effcce738da5f49a8333ab3d058a5ad7' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
