ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0-alpine3.13-amd64

# Install .NET
ENV DOTNET_VERSION=6.0.0-preview.3.21180.7

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='dec32010b4b0ca3b506ed1b9e3e9a9be3d90788ecd3dcce764891c95a6dc0ae0bb375caa805b7ef1ee34d503f7b1812d5f7cec2ceefb611cecc68304ea229bf5' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
