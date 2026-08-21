ARG REPO=mcr.microsoft.com/dotnet/sdk
FROM $REPO:11.0.100-rc.1-azurelinux3.0-arm64v8

RUN tdnf install -y \
        binutils \
        gcc \
        glibc-devel \
        zlib-devel \
    && tdnf clean all
