ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.20-cbl-mariner1.0

# Install .NET Core
RUN dotnet_version=3.1.20 \
    && curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-host-$dotnet_version-x64.rpm \
    && dotnet_sha512='a07deccbd9e65453dff2b34d5c5c4d05d34a352071e4931bfbf2fae1e7b0062ca35dcdd874be75889ee77369acb7deabd08c2753c5a7219a9e1e5a4ec43fd2b0' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-hostfxr-$dotnet_version-x64.rpm \
    && dotnet_sha512='f8483b67ebe1242dfac781b519ccc7d5dc9f54183adadcee400004b49bd611600f038ee5bb6f555b9f0537f45f84b8276ae969d6833afee8f386150772659b33' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-x64.rpm \
    && dotnet_sha512='d092411620402d03d49382267f5f87f55131cb9a4fffd440d7b4e8beef6ed922ec3dcffc5cb13a4b91457d1d66e99b63372eb7222f81248ca551855d810393a5' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
