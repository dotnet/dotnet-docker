ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        unzip \
    && tdnf clean all

# Retrieve YARP
RUN yarp_version=3.0.0-preview.1.25351.1 \
    && curl --fail --show-error --location --output yarp.zip https://ci.dot.net/public/reverse-proxy/$yarp_version/reverse-proxy-linux-arm64.zip \
    && yarp_sha512='ccff375ec74a9d7e96fb6fc64100773c67c72c7039c3109b4dc4925a9950215438dcebf974275e4bd86af7728041ef7f404f4d3e5cf6b2e575a10c39c86fc7d5' \
    && echo "$yarp_sha512  yarp.zip" | sha512sum -c - \
    && mkdir --parents /app \
    && unzip yarp.zip -d /app \
    && rm yarp.zip


# YARP image
FROM $REPO:9.0.7-azurelinux3.0-distroless-arm64v8

COPY --from=installer ["/app", "/app"]

ENV \
    # Unset ASPNETCORE_HTTP_PORTS from base image
    ASPNETCORE_HTTP_PORTS= \
    # yarp environment variables
    ASPNETCORE_URLS=http://+:5000

ENTRYPOINT [ "dotnet", "/app/yarp.dll", "/etc/yarp.config" ]
