ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.23-cbl-mariner1.0

# Install .NET Core Runtime
RUN dotnet_version=3.1.23 \
    && curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-host-$dotnet_version-x64.rpm \
    && dotnet_sha512='a8bc42e86ccf7308744cdee395c2405bbb6c3e6c0436c2ad42b47b37765d83f0e57d5a4a47afb2545576708d6de6565ec0965780b6449fdf9976f994c56d813c' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-hostfxr-$dotnet_version-x64.rpm \
    && dotnet_sha512='f09b2120d6ead5fc2222785d4cc6980d61c21be10d20c8d9bb28750c3b141e7fca0587ebe1bc4a49610e82b52d1c08e7706d7ece533c239cf72f31b9d36e09ce' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-x64.rpm \
    && dotnet_sha512='777186fe5e73f0896d129910c03ec5f491909552311feb0a715537f2cd8d064a66175feb9f68f94cacf1943b38683fe320a0b5e696380a88b6f11493cdb0a39e' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
