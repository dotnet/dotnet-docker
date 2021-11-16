ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-preview.1-cbl-mariner1.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-preview.1.22076.8

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='29320b63f3c2bba9b01b6deefb4b440dcdf5799720b283ec8680b7d7c456b6f5f1147af92c073f7351076a79bc447bf8bbec58570442388b850b97af1e62675d' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='86b854f64258ebfee19011a4f0c174a38f36063ee62e1fa9e2043c769da57f557e3a24965a14a9de8accc4247c0f5ccd462038241c63efb0ad350b1a4f7ff36c' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='082897965d0a6aa8a0a4f9bf7f4a961865edf8b64b2ab509ce0bc51598b356da635e069d0bdf6c971dfa25bd25e28a521451685f9c6af554075134aceed13bbc' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
