ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        unzip \
    && tdnf clean all

# Retrieve YARP
RUN yarp_version=3.0.0-preview.1.25373.1 \
    && curl --fail --show-error --location --output yarp.zip https://ci.dot.net/public/reverse-proxy/$yarp_version/reverse-proxy-linux-arm64.zip \
    && yarp_sha512='db90bd806de2592a87860e295d5b10ec382e708f3384ac0e740cc73811b6866c5cb01947bda8afa961ccb2fda5f9c1784fe76e47b82db01da836bc09e027cc41' \
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
