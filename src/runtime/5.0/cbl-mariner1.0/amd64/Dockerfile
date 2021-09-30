ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:5.0.10-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=5.0.10

RUN curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='c01310244218ddf9bf8cef36a3884852d79a5bdf5be3c1d33f848944cfc331ba44fab673b4436a788d2b6dffd9278e91b4c64fb9885d118d15fdc501325144cc' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='3395176a279fd72d88e8b37d588250a2ce07f51178233959ae4b2fbcc457c436168397e2b5a31006e21f2b97885dbc5df6187b8f37f51cf7166905d2b19fb454' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='54cc1a52a4d177146959bcc6a6ca4ac32f76fb63fc3109516858aa25205126d73f517b1e9adbaccbf67666d3ec7d0ff8da82f9c86eb4c0bc34825b67c9f46600' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
