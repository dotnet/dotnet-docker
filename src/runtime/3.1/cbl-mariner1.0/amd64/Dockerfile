ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.28-cbl-mariner1.0

# Install .NET Core Runtime
RUN dotnet_version=3.1.28 \
    && curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-host-$dotnet_version-x64.rpm \
    && dotnet_sha512='52c34a8d619dc96bee68b9fa099231fc60860d54008f34cbf840de57f931048a31e2e76bf493f07af26314aca63f0451c2068494e168fcee99e20ebe635fbf72' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-hostfxr-$dotnet_version-x64.rpm \
    && dotnet_sha512='eb50adc5cc1d6b2d765926e52032677069cade7d1477d4f6f89fde3d89f19475bf45026b57dce60de77a5e0a3b52cad0cc90e2385d14b5818f41ad2107d27367' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-x64.rpm \
    && dotnet_sha512='beb51cddd836069cbd9bfe148b62383eead11bb8a448e101cf320b02a5650b75d82111745fa912aa3b1007f4e25fefd33617a323fa220deda90bb02be0ffe379' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
