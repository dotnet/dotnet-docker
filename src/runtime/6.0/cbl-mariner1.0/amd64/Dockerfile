ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.7-cbl-mariner1.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.7-servicing.22322.2

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='b2d06b78d737a309730ff405f7a95ff837aa196dd1dcb8e0e1cf55eeba138587579121f67ab6581d8f9e91852095829464779ce8902c578c2e0efda6d34d3e6b' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='037914cf2af3884ae9694f182ea47a2648e502953775676100dfbd5b9b3f34fa190513bebccb2d88df107fe0aadd3b62789660ae42c938900d0503b37b166441' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='3071124bef49b59970d7d396a4c120987dd6b5e0ad1c4e6577f89a52d256565ae290cdbe1e7f72d7c9e40a73739e4c63676a65749d267d06054baad22b9e8f94' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
