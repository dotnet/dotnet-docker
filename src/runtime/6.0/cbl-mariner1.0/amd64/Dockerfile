ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.0-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=6.0.0

RUN curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rtm.21522.10/dotnet-host-6.0.0-x64.rpm \
    && dotnet_sha512='bfcc264b1aeb4f94782ef21d31e8e1e9784fee282acae9d229910285313c658bd868ef730ac822bd60a7f978a07ae11e199689fea650090945f3e7b239d76db2' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rtm.21522.10/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='1b104322e2df4ef7869ad6eabd666903f64eddb050a6a359bf1efd74aa7bf4e4b6a23b003e695e74ff3c74813ca6019189c1c44fb23893e2271c449308108fe9' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rtm.21522.10/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='bcfa7d5f5de51fcfa51aec67088d46b8e5dcdab7a0b1fd96b1aec26f99f7fb78c57626d4bcefeccd6d1406d58bc7bd604d6740e1a8fe24cbbe47d344088da736' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
