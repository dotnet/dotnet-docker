ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.26-cbl-mariner1.0

# Install .NET Core Runtime
RUN dotnet_version=3.1.26 \
    && curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-host-$dotnet_version-x64.rpm \
    && dotnet_sha512='bac55ff96b4514c8ad4b8e9d40420d43e2331b7c81b972341e316b395ae93bee2cffa3664fe917c3fdb954db9abe5fba1aa4af80015415cef986b40de6b3d850' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-hostfxr-$dotnet_version-x64.rpm \
    && dotnet_sha512='8f630235bcd2ad3f9b58e3f5a0f29055067eed88c6d0aa01b1927a2a18190fe2a30cb339f2922408a1ed1c37d8eb8d45bd267891b676e70d77fe1696ff0a26ba' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-x64.rpm \
    && dotnet_sha512='aa73bc2d56ed806ac0fddcfa51c25af7e2ceb236dacfc0de3f5557da5da571f870e452dcc8ccb092ad170fcdd489ea5b15c8af2af761cf27d1cee6e6d3811459' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
