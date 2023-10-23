ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.24-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.24

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='1b77a571c93deeea05a9be832294ebaf2153576ed8c557f17e5d17ae0c5d3bee05c3c9544810a9b3311a3bf8ecc385f44a88c86ac2705eb5bd08e9421e900953' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='0e9d8f72c9b74fbe375b9c06da67ffa49bb0f10d78f428e7d8ad3ea009f01223572e5a2b4a39abb19866ba8ba32c3585060793532f868ca897f40b1cbee780ff' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='133957ab52a03dc4ca0da990bd6899260906a127c772ec1191ccaea59a7a6fbeffaa2578bad5f1d947dccf08448ef3a58cf456ac4ee565003325aa0a7034c8ae' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
