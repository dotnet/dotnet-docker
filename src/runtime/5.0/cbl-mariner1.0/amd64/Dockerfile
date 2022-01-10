ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:5.0.13-cbl-mariner1.0-amd64

ENV DOTNET_VERSION=5.0.13

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='e31e84e5a7b900ee23efd3df8817616a05d12c6da28c7ba38fc5183c98052a1a51d56f24b6e9137b0644aa74a6212200e4cc178212e81b0ca3b52733a0b34769' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='c445c69dbfc7b1dfed1027f19cc1902956fb13e1f07fd0228d32c2a63d3967c0aac6cffc05372b616c0e96f3ce29ee5e42c87f3bcf1a9f8ca0b31abb7daa1733' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='18175d5c89db6aa79c7c9cf00bec56fabe951e507342022a7998a21203b59c5d8cec9504f6c708bfbc7974865b45e542af33b41549f5b94789e6177dc3a3805c' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
