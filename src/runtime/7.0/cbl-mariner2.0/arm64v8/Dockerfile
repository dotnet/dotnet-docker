ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-preview.5-cbl-mariner2.0-arm64v8

RUN tdnf install -y \
        ca-certificates-microsoft \
        gzip \
        tar \
    && tdnf clean all

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-preview.5.22301.12

# Install .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-arm64.tar.gz \
    && dotnet_sha512='0751be17efa3191e6c9dd3bf0b7f1f8fd21028282b9eacf6786c6f61c7898199d15ad4719686e0437e4ecdd0c2d85830344732afb5e2f4579e89aa410f75ee4f' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
