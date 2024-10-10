ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.35-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.35

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='ac1182adb3529dbbb568677257ad77336d16f65dd50d3533e1ffbac3a908608dc5d3a32ce6a7add2211a3dc1766e043c8c8c8bd3787ebbae3525951b6951ba27' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='37401ae7fc0547897a005b28def2e1abe0c9b4f452c2d575057da89ba9febeb7c08c224a74327ca8c6ba06947e70fc5ad644c6389ab42b454c84d8db57aee928' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='cecd4cd9c081bd37dfa9413cc646ff7300096c78b5f09038b54b7564559518a9813668b94653b11ab0346186cab05964c06476b5c10883c5969d88106fe8cad7' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
