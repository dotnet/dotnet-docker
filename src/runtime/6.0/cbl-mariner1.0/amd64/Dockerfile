ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=6.0.0-preview.6.21325.7

RUN curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='5e89ec11c7ccb028ef57eb6b7036e04272bc1ed10bbdf941ef58ecf310eb021614466ece372042ad833bd34bc8f86e931c209e849e5fa7cc1122f435b1fe4f80' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='206c616ec015d5a57181c3329c78fb198e679172bd583baa29b4dec1e7861b47bb5e74d38c17158d8e56d33006aad3d8f66a4b4ceb53242e2b5fb7e1958bc411' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='70d133b47ed7dfb1681472d8bfb004afddd60c157ff795e81cc87d01419a801ee9a02b8b0b91697577f0a53d03b9c9d22a775a8d81c2cd867ffd7582e3870577' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    \
    ## Manually configure symbol link due to https://github.com/dotnet/runtime/issues/53543
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
