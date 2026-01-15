ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve YARP
RUN yarp_version=3.0.0-preview.1.25375.1 \
    && curl --fail --show-error --location \
        --remote-name https://ci.dot.net/public/reverse-proxy/$yarp_version/reverse-proxy-linux-x64.tar.gz \
        --remote-name https://ci.dot.net/public-checksums/reverse-proxy/$yarp_version/reverse-proxy-linux-x64.tar.gz.sha512 \
    && echo "$(cat reverse-proxy-linux-x64.tar.gz.sha512)  reverse-proxy-linux-x64.tar.gz" | sha512sum -c - \
    && mkdir --parents /app \
    && tar --gzip --extract --no-same-owner --file reverse-proxy-linux-x64.tar.gz --directory /app \
    && rm  \
        reverse-proxy-linux-x64.tar.gz \
        reverse-proxy-linux-x64.tar.gz.sha512


# YARP image
FROM $REPO:9.0.12-azurelinux3.0-distroless-amd64

COPY --from=installer ["/app", "/app"]

ENV \
    # Unset ASPNETCORE_HTTP_PORTS from base image
    ASPNETCORE_HTTP_PORTS= \
    # yarp environment variables
    ASPNETCORE_URLS=http://+:5000

ENTRYPOINT [ "dotnet", "/app/yarp.dll", "/etc/yarp.config" ]
