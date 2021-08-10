ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.18-cbl-mariner1.0

# Install .NET Core
RUN dotnet_version=3.1.18 \
    && curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-host-$dotnet_version-x64.rpm \
    && dotnet_sha512='f4b46f377e98fd8ad651c21ddbf62da5127e60f71ef583b8794162fd817f40d8a6af4939da5b8f74dccd35d63117feec5beca3626339dbb227793514847c3da7' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-hostfxr-$dotnet_version-x64.rpm \
    && dotnet_sha512='8a6d72017744823b0e8e60bf2437a1f4c04b4499123786efbfc6e16f63f165b0f3e37bcecdf73a14df00dae47916b0e62ff4857b1ac98e8ce5d3bc95b2b8ee54' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-x64.rpm \
    && dotnet_sha512='eea4c35d9a7ca6cd8d69e2aac72e9e5b23314fcab6ff93e7b09687c261a031bff1ae24abc074d20ddef9462db16d8931d364405253e1c94309fffd864c11e10e' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
