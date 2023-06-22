ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:8.0.0-preview.6-cbl-mariner2.0-arm64v8 AS installer

RUN tdnf install -y \
        tar \
    && tdnf clean all

# Retrieve .NET Runtime
RUN dotnet_version=8.0.0-preview.6.23321.2 \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm64.tar.gz \
    && dotnet_sha512='63ba7d05f1753fb954a0923317532f5c4450226d4a706c949e03d7e15dd0b592c934cd2c1cfcc86af9361b7901702e242f935c0c81be333c31c9942f42a54e6b' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:8.0.0-preview.6-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=8.0.0-preview.6.23321.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
