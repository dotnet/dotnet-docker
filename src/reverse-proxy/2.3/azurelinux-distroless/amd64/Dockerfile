ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        unzip \
    && tdnf clean all

# Retrieve YARP
RUN yarp_version=2.3.0-preview.1.25060.1 \
    && curl -fSL --output reverse-proxy.zip https://ci.dot.net/public/reverse-proxy/$yarp_version/reverse-proxy-linux-x64.zip \
    && yarp_sha512='24bc79fb64105f7a910e3deb176abdd55ffda05e4885d66309d62912159cd153c5d140b5bfb05d84141282fd13e7f6559402c8c3046b3c6c4f2039b52ee1aa7b' \
    && echo "$yarp_sha512  reverse-proxy.zip" | sha512sum -c - \
    && mkdir -p /app \
    && unzip reverse-proxy.zip -d /app \
    && rm reverse-proxy.zip


# YARP image
FROM $REPO:9.0.1-azurelinux3.0-distroless-amd64

WORKDIR /app
COPY --from=installer /app .

ENV \
    # Unset ASPNETCORE_HTTP_PORTS from base image
    ASPNETCORE_HTTP_PORTS= \
    # reverse-proxy environment variables
    ASPNETCORE_URLS=http://+:5000

ENTRYPOINT [ "dotnet", "/app/yarp.dll", "/etc/reverse-proxy.config" ]
