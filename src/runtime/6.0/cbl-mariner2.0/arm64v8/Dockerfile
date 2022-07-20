ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.7-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.7-servicing.22322.2

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='037223c13297dcebda06fcf644d97320f4f5a19dcb565401ab32c5a7e1c94d4bf26e8cb0581f064eada74cee1706e3820d77988721a577e6adfd5b5a87a598ab' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='f92a1d0f1d8b0087e11def96ba56e866d3e52036a51b2489ed63427f090fcac6e59094c43598844dcbaa3ac87cfa661e36adae934de2405466f79bb9b167a9f9' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='0808c7767ebc2903933d1ff48f96b1069951ecbe4ccd12c359bbc82355c745c984fc2c8d5d7dd2f7d5203b39d46d1162adaa4264d6bffde95ac4be233907419d' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
