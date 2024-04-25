ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.29-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.29

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='eeb120437bcf0d1db0029fc6f15318a2b76199d4950d083c052df5c2a894a923463f2093d254c4d4b1eb41838420b41216a6e958dfe64bdf797dee6c6dffa153' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='885e25ff6cb06f227736f0fab0ace6a1dfe4ac063c498822fcf79a5021745d34b01f1e2dfa9b29e0f8a2d2d63e696a3ac50d712c8ae6d5b39f47844138689c62' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='d37715497753603552ac7d730e0d9a888d25bb830846abe50ff100a0f3b0e3992ff64742a9527900faf252bd02799f85846cf0627b560a1a46d1177fba5464ce' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
