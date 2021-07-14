ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=6.0.0-preview.6.21352.12

RUN curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='35e348fc5678b6372478192e40dbc4456d26b24635cc7f2e49f4d2a0e602cf44541840ebef6edf70459092be29ddfa068f0ebb485df4fc65e442a7c24fc3ef57' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='9a25fec3226169508bb3a69196e97008ad50fcab6289fb3aff2dcf252241deb2d8d7069a88572a4be1a70d2c3fbfdad96f411929de7b00160fe2780fdeaf23b3' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='793bada837e6ba1ef2743673b8a78cd188754a47ea6fb4bde2802e1ef609ff5a5dd6996101c654504468d28d93055c2a4843cf950517e3c6f696cfd80455b732' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    \
    ## Manually configure symbol link due to https://github.com/dotnet/runtime/issues/53543
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
