ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:5.0-alpine3.12-arm32v7

# Install .NET
ENV DOTNET_VERSION 5.0.1

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm.tar.gz \
    && dotnet_sha512='a5a40d3df4c68fe5ccc2344a8cf9a875c069b9d26328f7fe920df02131e5b1fde7ce31274edd1a01a0ebed6550a0d5df8a456d4b89ab6b9abae6b5da837f4d84' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
