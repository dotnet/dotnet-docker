ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:3.0-alpine3.10-arm64v8

# Install .NET Core
ENV DOTNET_VERSION 3.0.0-rc1-19454-17

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='5f93b5b3688430fac80f931b1b8532dcc2b71d46fd67ce0150d9c5d80024363adc49d087676e9369bd8f54ca0f914cb0cc0bcdd45bd8ef8e4bfe9b764cb67932' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
