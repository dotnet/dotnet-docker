ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:3.0-alpine3.9

# Install .NET Core
ENV DOTNET_VERSION 3.0.0-preview7-27816-04

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='02133b6bef6a209314e6650d3badb301d7ebec2aa740cdff5ab247adcb2286a74fafdf04036092f949268507cb7ff859f51bd0bfd5d3480ebe32f2029f8fc2d6' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
