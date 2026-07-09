ARG REPO=mcr.microsoft.com/dotnet/sdk
FROM $REPO:11.0.100-preview.7-azurelinux3.0-amd64

RUN tdnf install -y \
        binutils \
        gcc \
        glibc-devel \
        zlib-devel \
    && tdnf clean all
