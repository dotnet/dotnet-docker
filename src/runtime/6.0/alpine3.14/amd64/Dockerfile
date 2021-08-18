ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.0-rc.1-alpine3.14-amd64

# Install .NET
ENV DOTNET_VERSION=6.0.0-rc.1.21417.1

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='378e16463b60ea7c506ef33755fe6af501c462a736f31d8aca7694c35c01dcfa03d4f08011a5b10aec0a457a34ef4812ac0028c8448612dcba4aa7a02cfa8b2e' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
