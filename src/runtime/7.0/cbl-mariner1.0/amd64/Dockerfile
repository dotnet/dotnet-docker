ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-alpha.1-cbl-mariner1.0-amd64

ENV DOTNET_VERSION=7.0.0-alpha.1.22061.11

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='b5f980a3c055629a22852cf1099e210550cfcab7edc5aab9b4dcde8f496b74b8a52860df6f2370d0276ed59c5fa6f23288d07df254c5393834043bc03ea4f7b6' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='b20570bc0827f1163b8bc1f43d5e6f7ec9828d8105492624ef45d307a45d4d9f12bba9274a11c0544751c8d30a2d40cb03fb4785b69df1b116a02b963fb699da' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='5658a8f65891ef1a9594d6469c08dee137edc1738b6cc8fee4ff00d03c42d3efe93cd2b9682ff2147a0577e48a4a7c8a159fb0b795dd2d92aaa772f37fa8cf88' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
