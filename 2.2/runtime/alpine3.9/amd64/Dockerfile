ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:2.2-alpine3.9

# Install .NET Core
ENV DOTNET_VERSION 2.2.3

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='b11e8731dd2e6b8738fb3a2762ed90de08df6661a8720ed76ef9429b99d763d0913ee100042a2995d72a13b50394a7e357397cecb23402c3104075efda04f62b' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
