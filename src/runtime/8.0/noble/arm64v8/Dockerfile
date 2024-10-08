ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:noble-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=8.0.10 \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm64.tar.gz \
    && dotnet_sha512='31599ffbca710247f4e03fe99b1098b287a0ed820a944b5a6ed22372651c97d67531c34abadbc52e59e8f70b4f76cd331221d008684f3feefd9be2904a73e388' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:8.0.10-noble-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=8.0.10

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
