ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-preview.5-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-preview.5.22272.3

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='717ca7f6cbc3cd1ebad899b3f682c5cb13ce843bb59dfc54837020f3cf5a28dfc888c29fe3dfec553d7a50b831dc8de8ad69459f47795d0d6e874f8194c29d45' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='3f4d2e112b5d133c6f016fdba22cd2089abd783cc660b408e37cc293a89a08d4a445617c6c9eb5ce4a22ed76d834621e96c00e1f332493086c4bcee023b049ed' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='cdb0e22fdfae05650fe94fe2d1db65f16df33a2910d6e12787c64a8e19b33f6f27219084f6a0598f715c14bc7185ae102289c53cc28efb5b6121c8d01c27ed67' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
