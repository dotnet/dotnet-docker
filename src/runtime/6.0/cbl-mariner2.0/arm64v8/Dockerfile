ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.8-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.8

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='dc2ef9d16efcbb4ed44ca3f8484603b0f064c0d0e8b181cb055daf0cfd46dc787c46a52c8a9331205748b0f5ed02d537709ed54a6c1d8ed376ff55e969060892' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='cf3de46a2014a6f917c8762af863682a0f70eaa53c61359f66c2ab5dd35f1f8d08e10a1da5c46adff2862bfa31d3763741582868450a8fe191f6b6b5b6cd4935' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='bb705bdf1a773c6f24624b605eb856e4d4a4aef76a2ead4ddc23e3ca9cb16f61b2b7a74cca5025d4b2006ab7d2b178e53cd9ca4ec1018191f64d4849a48ef100' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
