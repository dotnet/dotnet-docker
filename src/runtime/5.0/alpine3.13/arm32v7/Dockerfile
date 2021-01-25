ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:5.0-alpine3.13-arm32v7

# Install .NET
ENV DOTNET_VERSION=5.0.2

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm.tar.gz \
    && dotnet_sha512='e608a714767c8ddb6c397a0bd67e493c8546cd0d47c4c393f95d16a9e6aef0949c535a5ab8f0ccfe0642456f29c69d1b9a99561b37b9d24180bcea7e9417d5eb' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
