ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-alpha.1-cbl-mariner1.0-amd64

ENV DOTNET_VERSION=7.0.0-alpha.1.22065.6

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='4efe12f6750f5fc3b20f76fb7754c21891a41e6325aab2dd8fd264568d236165e19d404cb895ffc0a5ec49b9ee3915f5c7f9f528c2a8c0e651ff8ede3505eec7' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='f1ada0fafa9c393a9da7c2d93f23d4b86ed59d3da1bbb5e6c79f73bfd94e6a4830422d83cd3b045961b8729196c43d6ca8ac7b5039224ec3c290b6bfe6937384' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='87ee68c93651f3908f00a66483fab330cae0dde9da4cc6302cc6e0fca9eccd2c0be215efb3290d7d6dea69a1d151145ebe75d22425aa0f13de8325df77b95730' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
