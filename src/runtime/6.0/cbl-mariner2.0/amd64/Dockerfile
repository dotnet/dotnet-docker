ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.14-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.14

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='aed224cb77dd9a698615b94797110af0db7e44c5a7bfcceee0b1b0a4c82a9b7f9d82a27a43ea781235e10b911067a6419e4cff21f7326d74112ceaf565c611e1' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='23874d89595c699f66f283b844582ffc99403d2436a6588374707c08ee6224b721d3cbc2fffb73eade7fff811f081856ccd9425e8058580652516b4d8d437201' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='a17c2e3f7b64ea303125491347c6fa14e6a3b9f112e8df6b0da9247b6bf2459d127baf518d45ff515959456586c3a45f2f4854dd2abd738069d777d7a814030e' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
