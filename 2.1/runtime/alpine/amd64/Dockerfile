FROM microsoft/dotnet-nightly:2.1-runtime-deps-alpine

# Install .NET Core
ENV DOTNET_VERSION 2.1.0-preview2-26330-03

RUN apk add --no-cache --virtual .build-deps \
        openssl \
    && wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-alpine.3.6-x64.tar.gz \
    && dotnet_sha512='111213ad08a1870b56719756bcf0d31aacbf8b61b5854b763785b4960051040a59576c5f2ae7de74d15967256ca65702929417c797d5c921c108361ac039a1bc' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz \
    && apk del .build-deps
