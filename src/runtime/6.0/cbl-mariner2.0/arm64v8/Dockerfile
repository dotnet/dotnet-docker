ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.25-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.25

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='f99f77aa406c8bde4e48f2233d1d578f7c60251ba75eeafb4de2f511acd22f20b037a810251aad1d55195638ad0ec245b86ca560c068355588a19be73cfc9bc9' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='4315e567fd66e1e91a3beaee43d243baf8308e836f68bf0fe46aa262e6eb363ebe8d6a86eda549351db2754ceaaccf46b2641b046d7797fb5a1429bc111ee6ef' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='ce083280f99ccb23dac4c0cb81434c5d8584a14068dbc525576f7a9c5876cd174f288a16793ca9f3d85caea772d9c6d2e15bac86ff8034e84b62251c16d39126' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
