FROM microsoft/dotnet-nightly:2.1-runtime-deps-alpine3.7

# Install .NET Core
ENV DOTNET_VERSION 2.1.0-rtm-26515-03

RUN apk add --no-cache --virtual .build-deps \
        openssl \
    && wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='e46b87612e41a72db919104aea58fabbdd8f294887471e119320e9fda50f05b1d321f61206abe2f9ab71dfe38c61f7df263aa93ea618568ac95f3260380dced2' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz \
    && apk del .build-deps
