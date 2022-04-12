ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.24-cbl-mariner1.0

# Install .NET Core Runtime
RUN dotnet_version=3.1.24 \
    && curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-host-$dotnet_version-x64.rpm \
    && dotnet_sha512='712c8963b9d08ad7d8abf72c1062ee7b372658b20d29b9f94429a6fdcc6192251a609bc04d470fe18226900685dc21ab9733df70c88c48e32951d677815bb5db' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-hostfxr-$dotnet_version-x64.rpm \
    && dotnet_sha512='e5cc0bae363acaf6c5530c497e50b167c854dd95f3ca82b20de9d9293ce53ab31ae8bfe29c361c6bdd7aa1c09079ba1587fd351c1250c4c1271684f21bb828de' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-x64.rpm \
    && dotnet_sha512='10a9eae512c1146e8437c030fc73375e3dec7b109502f25815f2bd9c38bd44887636651d482f99087ae428b14d8a3f17905c8db34c4808077a8572a0b78f8e9b' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
