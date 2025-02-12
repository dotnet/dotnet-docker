ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        unzip \
    && tdnf clean all

# Retrieve YARP
RUN yarp_version=2.3.0-preview.1.25060.1 \
    && curl -fSL --output reverse-proxy.zip https://ci.dot.net/public/reverse-proxy/$yarp_version/reverse-proxy-linux-arm64.zip \
    && yarp_sha512='5ade6d5e05328328f50137d5c796e45ff7018ec60b783ddf03ae5ff0fa7577bfd9458201ce22eb01709d2ff98e35bbf522e3075487e4ec859844ae10880079a2' \
    && echo "$yarp_sha512  reverse-proxy.zip" | sha512sum -c - \
    && mkdir -p /app \
    && unzip reverse-proxy.zip -d /app \
    && rm reverse-proxy.zip


# YARP image
FROM $REPO:9.0.2-azurelinux3.0-distroless-arm64v8

COPY --from=installer ["/app", "/app"]

ENV \
    # Unset ASPNETCORE_HTTP_PORTS from base image
    ASPNETCORE_HTTP_PORTS= \
    # reverse-proxy environment variables
    ASPNETCORE_URLS=http://+:5000

ENTRYPOINT [ "dotnet", "/app/yarp.dll", "/etc/reverse-proxy.config" ]
