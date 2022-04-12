ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:5.0.16-cbl-mariner1.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=5.0.16

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='34e90a72d9fe83a2f9fc6f15d332557dabdc808366c94cc9661b84fc1bc8515c54bad791ab254c4f8970640ee4e6914c23534c01a5eaab25ecf240113f1b18cd' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='96923af17f854c37fe2bf49cb324d0fc186f3e672671b0e5158c1bdbcbd844d0fc2eb3b3e4eb7b5c27eddeedbfcea590de96a3b23e58b793b20d2a44f1126b7e' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='dd2cd7a709a237a0ef118aa0170c5c00d1845f14e2bb9d40503a602aa24f15ca05f0c55ec4c5c5f3b3d89073888c390ae473dbdf487ca37ea742dfae68aa2aa1' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
