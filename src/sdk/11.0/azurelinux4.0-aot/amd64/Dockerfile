ARG REPO=mcr.microsoft.com/dotnet/sdk
FROM $REPO:11.0.100-preview.4-azurelinux4.0-amd64

RUN dnf install -y \
        build-essential \
        clang \
        zlib-devel \
    && dnf clean all
