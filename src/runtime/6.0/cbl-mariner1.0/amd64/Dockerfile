ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.3-cbl-mariner1.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.3

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='4ff2a0a9197bb86fbaf5a9cefb588dad12930f68b9486de4f44494e6109a9016ece0a09a05718ae75c58507e371b0d5fc610efbb23060e58609df47a1798247e' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='bc7cceaa4bd93fb57f27e139a8138cee72679c539cf0dce53695256856242735568208bfc780049870261a08086936d0af0dac79ce42e09e805e6d413fda0b36' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='9ff31f20e56dbe1107bf5074e4a7f06084cd2ae346c80aaa10d6c9f82c4a43a694be0fc807bc6c3451bc7854908f82fe4c3be3261a13bb41082c891cec64db88' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
