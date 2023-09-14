ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.22-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.22

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='b135b5f6bd9b46f43b11c50017f47f7f5e609bb0560d08a865d39989794c60c222a7d71b675f59557af43083c6f550ffa60a1063826dcd8b9e625e47b54b55dd' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='6e7b1cb5234ff6a7c8eb07b44a9af610853e4ba6765c7b2f7083eee0b9b125adf9f5222bb5d279feda2ebe3e08f6cd499e044824b97476801ce3a7b247cfe6b9' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='99f7a8e9d1c85d59795a046d09a4370ed61a07857be8e0722b393a0143bfe3da398e553a52a5a66db34ba8e9ac0532f227e64d2799345e47042803cb30caba5d' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
