ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:5.0.15-cbl-mariner1.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=5.0.15

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='8f37e84d85ce2bda86a5da9333e7696bb708deed8c09366982f52831ebbdcedf9b80e53b48635d8149f18843d26f36a1e0eb0e3c7df7bb68d8102661e3f26106' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='51568eeffcc4ddffeb4c562f3f8f25d3f74ddd4c196dca1bf89ca0374d00f28405150d22e8934460a360f5121ca316be7bc8cea3e207d7ec8f6cac7116a28617' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='51599340056b923931e7077342b769b46c7d796fd840bf45c78ad6c9402e26aad813037d459ee472cf8ed1e4a1fc608b2c1ad17350c62acc9bf4b65e4b8a414e' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
