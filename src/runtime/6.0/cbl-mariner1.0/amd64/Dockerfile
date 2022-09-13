ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.9-cbl-mariner1.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.9

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='a69abd2a4be3bd0337098c7a62c1d77a9f2309e13aa84207f23e8bea7e8b498941b4408328cfc54c3c8937482c18902f9d4a8b0de630bf3f059ca42ec8bb01c6' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='49adcd3afdfc2acb70026dc52371e1b8bc0a768daf31faa602dd19a9b803b51ea6c9f08a3bab69325c43cfe206fe355d545f687bd52b9347f3baccfcaaa1e184' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='3273472fdc82de49ce7640642a26bb0f0d06a1a5eb35a6c956959e2435e81027c8434e539f8b463e6107f14daef337f99652e0380f0941f2cb654d5e5c29f2ed' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
