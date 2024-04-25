ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.29-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.29

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='2fba4a9894d2b4d6b515a5fc8872ecca0e043ac58e9fa9fa75f3117c83ceae099bf2d7468eb54e9c9571417536561138ed34e399df19f7b5b35582558ba341a9' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='6c7222b5313d49c897fe72a7e4a3ab790f51bc915b5667adb544b388b9b901c63bcc9793a8c10a1ed61becad6ba6f2e4f9aac9bd1fb7804118ef97f2ce8215bf' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='5591e450abbcd3a5497d51bff978241a716b4a3993d229fe8dfa2de2d3129d100faf5a233a5f4fbb9cabbcced51be37b212c85b29cd12c0b9db98e9f2ae14786' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
