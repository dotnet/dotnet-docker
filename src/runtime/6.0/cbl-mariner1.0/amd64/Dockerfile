ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.0-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=6.0.0

RUN curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-6.0.0-x64.rpm \
    && dotnet_sha512='eaa29e3a68bfb589fc917f26a2385220dcb6efb40696d357c29b9bba2018f9e614c1f0dd1b3b73945ba9c983ffc8cfe8de00c96ab47e00d2a5d1ed7782c96283' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='c74771f27b5895af93e61733bd1552b0da49dd6271f04f20fcac0856fb838c1f4248e54d3dc698d13e8ccc1ac7027c0a48cbdff36e3802307bf48a60ed62ab7a' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='9d4a14076aff38d12fa0a5142494176081eb1a2c116ffe48c80b8abc72aa68e2340ce238b55ece5aaa427362002b223ccbb94f8ca41fc8969ce7760cdade2fab' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
