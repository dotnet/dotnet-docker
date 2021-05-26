ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0-alpine3.13-arm64v8

# Install .NET
ENV DOTNET_VERSION=6.0.0-preview.5.21270.12

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='9182e626e52fcdcf4b1163b5cb0222b82dc316040da49c50b6f010c0f730d3fb671cca36ac782d700bdda318d3548925e08b52b7ce6e9aaecb922b16adb499e3' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
