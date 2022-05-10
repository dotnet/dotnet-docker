ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.5-cbl-mariner1.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.5

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='24bd2fe3ec4620929eca877aa5d5fd6335891027e2cce1180a4294bdb167f3d25801953fc25f203ca8711502e4d11e1ff8e5a6e6451d845faea2573c0b0b1c72' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='8ed43a4240b2263107a30b58ac325dca9f203023053948b7a2d29c3bbd9f79b80130645ed88dec66a5d5e7b1b2dc972aef0efb0a720c3f27a87164dd052ae04a' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='b69ee0eda65e1187e93a765ee5a3c6774fc780a78f11f8fe25858e4f9d92a42df8273cd083e3bc2f40d0e5318a5b6101f7f1fed7942f795f25c9891c7e4bb4dd' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
