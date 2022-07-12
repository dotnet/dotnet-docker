ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.27-cbl-mariner1.0

# Install .NET Core Runtime
RUN dotnet_version=3.1.27 \
    && curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-host-$dotnet_version-x64.rpm \
    && dotnet_sha512='eea7b97ae7c8856b17f3d3666710fdc60650d7ed087738819c6947854bbec9e40b24b883837cc2835f1c3bce61b650265cdbcab2fcf6a04cbad2543a605e711c' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-hostfxr-$dotnet_version-x64.rpm \
    && dotnet_sha512='cb06091c2a8e2f5d99368a7eb80ed54ab7159adc8c0e4c9bd1d925e8e87ae6dde22b14209821cbb00b320f154b7c3628b2f51c02f02908421879f000ce7843da' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-x64.rpm \
    && dotnet_sha512='1f52fd35ea6e9eb176e255fa161642bd1071f6f5f12c78a119ca58b0f618604823f2b4377a1a4bee7aa77f9be48b8410b405812a81fd89b92bfe935982895f57' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
