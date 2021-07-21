ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.0-preview.7-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=6.0.0-preview.7.21370.18

RUN curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='4beef8989ff9914c38979890610adeb4f780ad4ece39a7afb0898dea14f890c17f358382e0840fd3d8c6e6058642105571a39bd8ebb538a1acca76bbbb639dc4' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='59034d2609a677796bfe28b72d8cc5f63c3dfcff1d95508c3dd38d056a31c116a3898e9b8e14cd22e3f2ce6978f4b4b37e460b166fb2adb529799cd6a1f01ec6' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='a8dd0db664bc907393a2feea8be6c70ba9266e620646d29c4ff49606228ac3c2b8e2d8419cf033c1e3222a330cb8511725b287fc8b84823c8bcc92cdfa5a5bdd' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    \
    ## Manually configure symbol link due to https://github.com/dotnet/runtime/issues/53543
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
