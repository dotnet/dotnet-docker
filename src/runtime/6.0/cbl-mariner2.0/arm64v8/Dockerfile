ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.31-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.31

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='9dbfc80c99c5a7877c6f68208e34c2ac68af2e0aaa83bc70995ab2ffa07a3523ed97e74357dfc9b8ed433a2761111637f5c6a1d3aa272d39c4c26d95f3417963' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='b0158c3c7dc53c8341da23d26e300c817e7648bc1fa669b89b11234a5f97fa5f12a87e37e5cd0628a9c3a0c990cc8a6c643b6e9e03fc5059ba5ff0ed364cafcd' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='7723e0e32cff308dc9e3dd225faa0fe67740d057bc5dd2feb5590ecdb72a5d2c632040f276353513c7029a2c9810ab7c9e5b8b1893ae9f4bec7024b18a86a9da' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
