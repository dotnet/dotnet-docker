ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.8-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.8

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='f9539754fbf5cda2c7fd5b92f62d36080330afb140a912b6e152210ccde4355da3da244ac03f5fdec0f5b7634e8429a91611177aae31b2d1587b966f32154c3b' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='84c6d13c1dbabcf4c558b41633c3503dc8e105ad2f30721371e111f22767a7a6a78c352098f0db271355af3100cd0c9f8adb92ad566a835aec66ba336810b439' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='fd63a77faab1c8c8c143d4f97906e089abad4ec29dc026fac30290498f798a920b77a32379f0a6ca485fb91eff19f960c90f0a5cae2f38c844c1033ed7f37d8e' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
