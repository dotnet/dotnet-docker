ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:10.0.0-preview.5-azurelinux3.0-arm64v8 AS installer

RUN tdnf install -y \
        tar \
    && tdnf clean all

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.5.25277.114 \
    && curl --fail --show-error --location --output dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm64.tar.gz \
    && dotnet_sha512='9ae24d6528c30242f537f2d3e3ad89b87ebe47bad441fd6730df5264d135a63705721b67f434e2251809013b25adb2e3734d650e233e6c871c579d713240a650' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet.tar.gz --directory /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:10.0.0-preview.5-azurelinux3.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.5.25277.114

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
