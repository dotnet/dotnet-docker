ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-preview.3-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-preview.3.22173.5

# Install .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-arm64.tar.gz \
    && dotnet_sha512='100b9fa67b775cc47bb3e1186eb9dbb66ffdd143b0eee64624e3fa0c34419e5a53790bb91f30055571fa5eee384309912c098e57d3a4e3dea5744bcb7563c3bc' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
