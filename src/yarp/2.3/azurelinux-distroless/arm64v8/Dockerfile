ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install --assumeyes \
        ca-certificates \
        unzip \
    && tdnf clean all

# Retrieve YARP
RUN yarp_version=3.0.0-preview.1.25171.2 \
    && curl --fail --show-error --location --output yarp.zip https://ci.dot.net/public/reverse-proxy/$yarp_version/reverse-proxy-linux-arm64.zip \
    && yarp_sha512='70ace0ebbe550b4d059ddf7582fcda3959d1c9d607f4da6ec91c0ff1ed4f340601a194f558f3346c18a1678327ea4ad8ad5e4df3fed4f3fedad2fdcfb76115f0' \
    && echo "$yarp_sha512  yarp.zip" | sha512sum --check - \
    && mkdir --parents /app \
    && unzip yarp.zip -d /app \
    && rm yarp.zip


# YARP image
FROM $REPO:9.0.6-azurelinux3.0-distroless-arm64v8

COPY --from=installer ["/app", "/app"]

ENV \
    # Unset ASPNETCORE_HTTP_PORTS from base image
    ASPNETCORE_HTTP_PORTS= \
    # yarp environment variables
    ASPNETCORE_URLS=http://+:5000

ENTRYPOINT [ "dotnet", "/app/yarp.dll", "/etc/yarp.config" ]
