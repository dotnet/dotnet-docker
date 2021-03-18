ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
ARG DOTNET_VERSION=6.0.0-preview.3.21167.1

# Installer image
FROM amd64/buildpack-deps:focal-curl as installer
ARG DOTNET_VERSION

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-x64.tar.gz \
    && dotnet_sha512='d718dddf45ec4d02420e748f964c6939e23610683fb6fd6cfb68b23959c16d93ea867df3c467aec047951c9038f25ba6709d86299dbb71d3fcbc5d55b37e7a77' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0-focal-amd64
ARG DOTNET_VERSION

ENV DOTNET_VERSION=$DOTNET_VERSION

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
