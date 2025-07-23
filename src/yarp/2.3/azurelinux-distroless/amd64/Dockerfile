ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        unzip \
    && tdnf clean all

# Retrieve YARP
RUN yarp_version=3.0.0-preview.1.25373.1 \
    && curl --fail --show-error --location --output yarp.zip https://ci.dot.net/public/reverse-proxy/$yarp_version/reverse-proxy-linux-x64.zip \
    && yarp_sha512='10776172585027337222f7a0e0c496cc942edbfdc934c16d61d874be9b6d313f620db3ca1cad7fd06fbbce0e2b35b74be0ce9cafeb56e0c7dbaacfa7c5609c33' \
    && echo "$yarp_sha512  yarp.zip" | sha512sum -c - \
    && mkdir --parents /app \
    && unzip yarp.zip -d /app \
    && rm yarp.zip


# YARP image
FROM $REPO:9.0.7-azurelinux3.0-distroless-amd64

COPY --from=installer ["/app", "/app"]

ENV \
    # Unset ASPNETCORE_HTTP_PORTS from base image
    ASPNETCORE_HTTP_PORTS= \
    # yarp environment variables
    ASPNETCORE_URLS=http://+:5000

ENTRYPOINT [ "dotnet", "/app/yarp.dll", "/etc/yarp.config" ]
