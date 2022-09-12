ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.29-cbl-mariner1.0

# Install .NET Core Runtime
RUN dotnet_version=3.1.29 \
    && curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-host-$dotnet_version-x64.rpm \
    && dotnet_sha512='0dfe1443f725fc04f603bae0877bf86d4cde54146a67e6d1bb96cbbfb5dafbf97b42406ebeaf777e8c6ac36bb76dbed6799fed8d174a00b0a3421f974d7f09fd' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-hostfxr-$dotnet_version-x64.rpm \
    && dotnet_sha512='bf79ff4ec9fc95fd03e0e6dd000498c7b86af908d9371d7c2fcfefb768eb9398f4759ef9bf232a56699e9a6f8a4db29c75f270d380bc97e0bd2aa389fa16b468' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-x64.rpm \
    && dotnet_sha512='55533f1525d13974f341fd9e84d1b3f411243b07f630108b3a0438b2ce7ad2340ce148c60e869069e21ea751b6d72f6511a63073874481b3dcc5eb29ff4bc4a1' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
