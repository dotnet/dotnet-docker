ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.30-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.30

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='c88665ce275ae2f13b8affb6189696f8b59c092f6cbd0babdf791e80e47394d72bec689557e60364ae88922f10add55ccd09b4870f6fcfb023bea38f1aa47417' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='7e5eab5c2a01fee6789fcf1ba794e9e018abc57ba3fa948bd7c3fcccfa93d1445db4fd07b3116f1d9ebdc2461fe99a95772df4bf6e8bec6970e62d81b3219dda' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='b300576976b803f4a9475892c732970d28acb6a6fcc9edda0604a756bc06db46caf02028b02916cb4e20742a22654b2f4c577556a3288fab1d058a78372ff39d' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
