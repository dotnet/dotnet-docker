ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.21-cbl-mariner1.0

# Install .NET Core
RUN dotnet_version=3.1.21 \
    && curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-host-$dotnet_version-x64.rpm \
    && dotnet_sha512='7905f110d556170740263d452769035eb38deaf9eaf7d8824fd394bc0e8a239bb76769a4086684de84979fc706e553167563c5e6d5107c8851b9b22b1dc23f9a' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-hostfxr-$dotnet_version-x64.rpm \
    && dotnet_sha512='20e12a5ff75b6d3e09e771745d252bf6767a21cebfe985a0665f62aefcb788849633b111f9a91386a32a6575f871dfe4013e49b5d01e15b42d9690275e754af7' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-x64.rpm \
    && dotnet_sha512='16a087d9a173a3a715fda38c79099ef9d1997f942650284be5d004568ad6cbd048e8b80a934b0cc861c42d0e02b2efdc6ead7853fb7ed6c11129ba6a76762f86' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
