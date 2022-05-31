ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-preview.5-cbl-mariner2.0-arm64v8

RUN tdnf install -y \
        ca-certificates-microsoft \
        gzip \
        tar \
    && tdnf clean all

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-preview.5.22272.3

# Install .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-arm64.tar.gz \
    && dotnet_sha512='e8e2d3d6e34fd9700adf2365582d55b33492340c0ae068f1cc481a1bba9beb6dd8a6f6c2b03925614a26b099380446a7ae4aa4cb2f8ffb0a5f3273c369f82b67' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
