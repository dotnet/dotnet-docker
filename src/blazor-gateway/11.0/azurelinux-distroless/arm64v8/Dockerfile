ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        unzip \
    && tdnf clean all

# Retrieve the Blazor Gateway CLI tool package. This package is architecture-neutral, so the
# same package and checksum are used to build every architecture of this image.
RUN gateway_version=11.0.0-preview.7.26381.103 \
    && curl --fail --show-error --location --output blazor_gateway.nupkg https://globalcdn.nuget.org/packages/microsoft.aspnetcore.components.gateway.cli.$gateway_version.nupkg \
    && blazor_gateway_sha512='20168120b8ea930a1d46bb6009f49c1f18cb578b840d742c3421b047f6e764e36daf4da19f751a8ba0c68f40701e19ff26f21b3e08dc5251ce3c565de3290850' \
    && echo "$blazor_gateway_sha512  blazor_gateway.nupkg" | sha512sum -c - \
    && mkdir --parents /tmp/blazor-gateway /app \
    && unzip blazor_gateway.nupkg "tools/net11.0/any/*" "THIRD-PARTY-NOTICES.txt" -d /tmp/blazor-gateway \
    && cp --recursive /tmp/blazor-gateway/tools/net11.0/any/. /app \
    && cp /tmp/blazor-gateway/THIRD-PARTY-NOTICES.txt /app \
    && rm --recursive --force \
        blazor_gateway.nupkg \
        /tmp/blazor-gateway \
        /app/blazor-gateway.exe \
        /app/DotnetToolSettings.xml \
        /app/web.config


# Blazor Gateway image
FROM $REPO:11.0.0-preview.7-azurelinux3.0-distroless-arm64v8

WORKDIR /app
COPY --from=installer /app .

ENTRYPOINT [ "dotnet", "/app/blazor-gateway.dll" ]
