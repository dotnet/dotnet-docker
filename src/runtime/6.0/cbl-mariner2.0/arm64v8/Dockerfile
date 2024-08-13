ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.33-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.33

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='ed2e6949618110db8b674c397195d629f3986249eb06862e93ff07a236019968a618b956256cfa8a5ef5b74f7b6609454d89dd52f6eb14ef48e8812b26e323a7' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='7b2a844e464870485621d9ed933658639c84b192ec34d365ad7d8304f598f192574ae090a778cecf2264d4895814b79fa2284c0bf15ff58cbf2927a051a0cbf8' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='8660c675b0764029587ae0e73bf2c4910458a73e67769b51c0f369f07c9b82ff32590d82afb56ea2147debb0daaf555144c05e1c54fe6ebe494c3a399b8a92c6' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
