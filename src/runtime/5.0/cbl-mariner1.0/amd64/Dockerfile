ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:5.0.8-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=5.0.8

RUN curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='624dfcfac71f209168b64b4b6f72739ad144595d3dcc4c53c36fdc5df7546912a8b68fc31a1f65a7a0ea07367272630717d4c5e8278642776c49ae07caea1882' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='067fd98f7467b9314cdb987e0789db1a21c716a69af21669270b6fc3592615697e6a980119a69790eebec232ca968fee632f65bfb256a9745a57cae82490a0a4' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='06ca32a926f5e6adaa5d7a40cd2722c5d1a8510e0fb1605ec375eb5f213afb218a02bb8e08a585b69662433df1e0825f413473e3f5d3f156d205abd8c30cb15b' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
