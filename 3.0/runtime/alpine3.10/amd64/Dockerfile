ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:3.0-alpine3.10

# Install .NET Core
ENV DOTNET_VERSION 3.0.0-preview9-19419-03

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='c87d6e2a936ef33dad39224dfb01ed5eadbb107224ffb9389cc207b24c76496d1a7d48f6ac1715d724fa80d6671ca76a4d9deae0577ac7a1b41a72efa5be6b57' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
