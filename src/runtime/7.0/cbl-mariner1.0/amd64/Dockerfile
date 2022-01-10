ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-alpha.1-cbl-mariner1.0-amd64

ENV DOTNET_VERSION=7.0.0-alpha.1.22053.15

# Install .NET
RUN curl -fSL --output dotnet-host.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='f6df81b2d074072e6901ecddc5973a1546c4d924bf2b70f99ba99c6af8be105d3fa9e004bacec361bfa412a6818114e91f6d7fbc922c509df556d5d34e885ace' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='44598c75291979d2950feb52dd14422419043cf0dc0b1bea4ff395adfabf372ea7f6047ab1070224cbdcfd4e5f86c1f385f83d3e2b034ede7545f7526604bc0a' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='ed097ac87af33ede113e83fa00bb07904fa1ada7dfc979dab0c167d7c086dd1a695049bc0266fc5720ef506f56d6e3f2e806f5f536a57394fd5e1226b6425044' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
