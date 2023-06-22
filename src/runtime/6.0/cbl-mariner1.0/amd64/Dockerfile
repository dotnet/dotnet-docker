ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.19-cbl-mariner1.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.19

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='0b9942f218ae8c1984e01f6cd5465108d3ad5c68ea400ded8f4d428b13d048a66ea13e002c56110699d577ee0d5a7e3d36d0beed5f0c9801874d3f5bccd8e0a9' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='03aae8e1bc144f2177c7b9c720f9709fadd8037ef29ee80776b2df7e246931e04208883c698a5c745afd686883126df15d92ce9655e0680a5e218f5521f8da78' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='bdea006661c2c7f88aba421e2a82c92a347e12d21cbe2798b2b46828a17c1cc6859acfe5d908f2fb45817fe405ba1d7ba321112dd0cecb09ae79fd1bde6775bd' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
