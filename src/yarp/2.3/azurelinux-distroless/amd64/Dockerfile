ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        unzip \
    && tdnf clean all

# Retrieve YARP
RUN yarp_version=3.0.0-preview.1.25171.2 \
    && curl -fSL --output yarp.zip https://ci.dot.net/public/reverse-proxy/$yarp_version/reverse-proxy-linux-x64.zip \
    && yarp_sha512='b7bcf3f7bb2feea5db4c51296000fdcab2a811349ea7466a056cc8eb40a78d588d853cbf7bdd10435f643667f999f0ee4697f34df063a1db8c5e11cf4d8b77b1' \
    && echo "$yarp_sha512  yarp.zip" | sha512sum -c - \
    && mkdir -p /app \
    && unzip yarp.zip -d /app \
    && rm yarp.zip


# YARP image
FROM $REPO:9.0.5-azurelinux3.0-distroless-amd64

COPY --from=installer ["/app", "/app"]

ENV \
    # Unset ASPNETCORE_HTTP_PORTS from base image
    ASPNETCORE_HTTP_PORTS= \
    # yarp environment variables
    ASPNETCORE_URLS=http://+:5000

ENTRYPOINT [ "dotnet", "/app/yarp.dll", "/etc/yarp.config" ]
