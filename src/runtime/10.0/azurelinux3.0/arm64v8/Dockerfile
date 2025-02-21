ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:10.0.0-preview.1-azurelinux3.0-arm64v8 AS installer

RUN tdnf install -y \
        tar \
    && tdnf clean all

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.1.25080.5 \
    && curl -fSL --output dotnet.tar.gz https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm64.tar.gz \
    && dotnet_sha512='840d229fc19322093ed5a18c123b5a322e92cad26926bdb709ee382076864a9e89a4dd0ecc13e5e48542b1732ebd19ba65cad1c1b63b94268d38e78bbe6b6b75' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:10.0.0-preview.1-azurelinux3.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.1.25080.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
