ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.13-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.13

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='82d57d01069184a5b6859bf5e5e0fed3657c1dc76b3639b94551ae06a82db0d38f30d2cfed7cb765350c02da85922d6e7b684512573455bf11ba6968313c734e' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='d60d9494675cb190acad00159e0d6e0e028cca937f32bbd511f553f095d8c5ead402d828a73314c09c6949d5a34b479cb4c8bb6940bacab49220f492722958af' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='5de0d51ab8cf15875246e0d8df9f1bc50ac9f6c6fb2bcb19ac7da9f0b22d0d093c6b6253ab8c11a224b9158b1650c4100eab04736ef452bfac55aa39de6342ff' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
