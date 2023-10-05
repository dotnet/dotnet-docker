ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.23-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.23

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='a34231ea0e21a5c38d6599d84e7a67d86a32ec405e9a2e53139c62dd16f2852b917af8f4dacc86e60a23a316bab5e14ba42e57fd17032d5de0b81a8c59612062' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='d934166f60bd0c93bec568b1efeb066e572a8bca7d46b1e884b2de142508e7e32648d95911168e83a5644f36a5244f8df112ce7df8fb463081d8bf2032cd20ce' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='00a3066d4b0d10ad7ddab3fb6d743a62469df2a8c65ae5e0b7e888a08f94425f516285f9aca7da0a9c35c4a50d0509027250112be14c60e6e2df811dd19d8612' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
