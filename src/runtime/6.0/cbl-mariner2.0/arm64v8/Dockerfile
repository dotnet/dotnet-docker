ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.9-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.9

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='a30d4cbe179cc1250ee7d00a6c51b67c552fa65b85018dc872b8046a3989d6ea60a11ecf3f1d4b06acbb48f2637505439b496b160598fe5dfa0e8a849e831697' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='5e67bd72f70054b056f41a1b48da8ac5cee535c82b64b6c2e4fe84a6911d2d6af18020919e9a0e0aacece5fbd6cb402166ca3964190e21da6c96e1b4434c0bae' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='a844aba9a52106df5b2eb8a8ca08bdacb02eb2805949a3c27e1f2276fff0678c74c052c4d1190a5e3499fc7f6e6fd1a80cbbb752a9a1f68034ec622c3202a2af' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
