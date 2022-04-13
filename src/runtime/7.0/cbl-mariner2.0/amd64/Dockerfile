ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-preview.3-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-preview.3.22175.4

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='2611e8fb745e99007a8c6f23e3099bdbf737fda553f92de9e12ef6e5d456c7188b9cac2bb68c20f34f64ebe0f4d238940b5c0ec14d9d3a45f963215f9b1466f4' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='ad048cdaf852c85e9eb64e0e99c0780fae1f0fe8c66bd71f3fc87cc5cc5e8d93bf97a04afe751e68c680dd6c4669a6f62c35bfc53b54ae094868342ded5f3d00' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='ec3786ee563562f292c7f272df2c1478d63ec013cfdf96a82bfcfe5902e5fb7bcb7f317a82bbdb26bfeae4b393cab805e5e01a42e88acd9f0ca29edaa76b57c6' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
