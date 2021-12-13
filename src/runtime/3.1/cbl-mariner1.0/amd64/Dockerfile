ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.22-cbl-mariner1.0

# Install .NET Core
RUN dotnet_version=3.1.22 \
    && curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-host-$dotnet_version-x64.rpm \
    && dotnet_sha512='7fa10ddbdf5cab7d418be0272f426a84ea978619bcb9591e40cd001aba077fdac8b2027b50810f4da4cba872d5dff6c42fb26e85ac5ec5c1d7507f3c58f01232' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-hostfxr-$dotnet_version-x64.rpm \
    && dotnet_sha512='b875b999e619ba504ee42ff8d273210e560a1113ba46fcf5b627b69d9483f305a848fd7e92abf138d0b649023621120d6096454bdc7d2fd2eb910cc42ee6e01b' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-x64.rpm \
    && dotnet_sha512='177fb052ede5d9183b7ae76e3a790b4faa3c87b0f41d1e8ce9825c690c7e6027e90e1cfc52df83270b9b0f8e288dca0ccfa554a5df7d74a53b496373107a0ec2' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
