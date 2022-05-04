ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.4-cbl-mariner2.0-arm64v8

RUN tdnf install -y \
        ca-certificates-microsoft \
        gzip \
        tar \
    && tdnf clean all

# .NET Runtime version
ENV DOTNET_VERSION=6.0.4

# Install .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-arm64.tar.gz \
    && dotnet_sha512='acbb50f2a1cde2bb8f59ec2059cd90f669748ce0da519ddbb831d8f279c4b896cc7a8f4275fb2c4726c5caf3d6430ee6d9823baa6f65238c7017ecbc2b8a6444' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
