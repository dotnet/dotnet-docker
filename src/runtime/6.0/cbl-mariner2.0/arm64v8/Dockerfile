ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.20-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.20

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='58168754a7837a94887c7a9495aaf47ae7e534568d42bce0489423dfe9c0184e63235232139473f1d44fe200b7a8fd04e431408ac924246006cd75fd89e4c1c4' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='3718ea3552da9053054a334222ecf10f5e9292e94458cd74260f9346d12bd9750aee220068ac230661df600a128d651a2632551e58aba8b9da2263ee144f55e5' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='c5699ae17fe364306f3a8028ab29b088fc5073cf8b7d60da9d5f6e50595d6e4adbf4fa67fdb79246acd019b2506842d844172288a7161aa5adb845da1cc56005' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
