ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=7.0.0-preview.2.22116.9 \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm64.tar.gz \
    && dotnet_sha512='122b2c636f2a3ecd0fc392274ba39b5751309e7baf963fdfb4a3c9e326cb35a43ae0429ebe5e8052b468b904fce2e27c70c43748adb4d4e78d0c44399fec294a' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-preview.2-focal-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-preview.2.22116.9

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
