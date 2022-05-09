ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.25-cbl-mariner1.0

# Install .NET Core Runtime
RUN dotnet_version=3.1.25 \
    && curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-host-$dotnet_version-x64.rpm \
    && dotnet_sha512='dc73fe235bb2afc2c9dd4f51e939c103405b506b4e8a09c5060b3d60138dedc6cb530e8976271b4118ee2d08740f3be3e332833d8fc64d504e111c8c9d375204' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-hostfxr-$dotnet_version-x64.rpm \
    && dotnet_sha512='49e02a80a9a717cc5ddd03ac25a390ff218c4214c7972f88dd64bfc1505dababe0d500b2ac1b7570031f9f4a8aa3ce3a4f399c026c185ba1d954767d5695644a' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-x64.rpm \
    && dotnet_sha512='9d1f6a9acdc4a48f233bf96bc96f8179f0186463f540ade756e06547e5dcd200cc316f920148d47415a387b8a2187cde519265e9a086f9d579d80609857aead2' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
