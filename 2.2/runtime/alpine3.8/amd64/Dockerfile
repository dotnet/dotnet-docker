FROM microsoft/dotnet-nightly:2.2-runtime-deps-alpine3.8

# Install .NET Core
ENV DOTNET_VERSION 2.2.0-preview3-26913-03

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='2bfe6ab9661d0cff9cf78b0243da17b9c9b1afe0bbd0f21b781c800f6971c33a27b98671543842e700894dbf5b10b5ae151c37acd441d00bead5b554aaeb41b3' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
