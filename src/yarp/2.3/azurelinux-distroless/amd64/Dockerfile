ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        unzip \
    && tdnf clean all

# Retrieve YARP
RUN yarp_version=3.0.0-preview.1.25351.1 \
    && curl --fail --show-error --location --output yarp.zip https://ci.dot.net/public/reverse-proxy/$yarp_version/reverse-proxy-linux-x64.zip \
    && yarp_sha512='b272dc7c90525aae3948dc340d5e4b83be7e290b8ebe5cdb6f5bc920cd59ead648f7dee119d5068a80bc359e0e0b6da2d589c8a99d68dd5db9f3a4c62992adb3' \
    && echo "$yarp_sha512  yarp.zip" | sha512sum -c - \
    && mkdir --parents /app \
    && unzip yarp.zip -d /app \
    && rm yarp.zip


# YARP image
FROM $REPO:9.0.6-azurelinux3.0-distroless-amd64

COPY --from=installer ["/app", "/app"]

ENV \
    # Unset ASPNETCORE_HTTP_PORTS from base image
    ASPNETCORE_HTTP_PORTS= \
    # yarp environment variables
    ASPNETCORE_URLS=http://+:5000

ENTRYPOINT [ "dotnet", "/app/yarp.dll", "/etc/yarp.config" ]
