ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.30-cbl-mariner1.0

# Install .NET Core Runtime
RUN dotnet_version=3.1.30 \
    && curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-host-$dotnet_version-x64.rpm \
    && dotnet_sha512='bfee74062c61a682a1e8147d975a62226593e8ee88c0125d9f5690f479cd4209fbecce861c0e23c33c7c98f84a42af672225dc62a500251c71ab430575a099f4' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-hostfxr-$dotnet_version-x64.rpm \
    && dotnet_sha512='41bae87326a53d749cfe380eb11e3df2e35971566d047b2e09cf984585d239f13fa9be1314648f88c723df970f96c902a2ba16c3ff9364263402407fc5ba0696' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-x64.rpm \
    && dotnet_sha512='1bbbf6ca05717699fb36c7781df81081b4da0e47b5b3f968c1ba4f09579716acb902d551e01527aaccdf4e607e71bc8ad15b6600e2e573336f25f41c8959d3a9' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
