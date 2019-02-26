ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:2.1-alpine3.7

# Install .NET Core
ENV DOTNET_VERSION 2.1.8

RUN apk add --no-cache --virtual .build-deps \
        openssl \
    && wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='38698a9047b695197e4437c284ed4806f7c6fc526807394fbd27dc42176d743c29f0f1d48023b457c5dd544ad14c0eca476c4666b7b907874e3816a897d96d3f' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz \
    && apk del .build-deps
