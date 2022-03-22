ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-preview.3-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-preview.3.22151.6

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='44bef8cd9ab5703ff715a11ea8c5e10b4a43a753594aa78333ea2cdc6985278ac069da38367ae8081442b5e85684fcba5aa529475bc1ab3789e45c8c73af00bf' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='dcbf508135f241f62f05c14233ffccaa2fee59fdd6ed151672c12e44866c7ef749c1e075ccb690aafe8a340ebf7633ea0266ef6533f43dcf2bef48332134723d' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='1c19ab40b3331daaa2485f1414afcd6dd159cf6955dc86a8e0d6a2c50c65d248a12b8d358740bcac245c1a677f40ece637f7a6da65ccef50e630a146311d7d2b' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
