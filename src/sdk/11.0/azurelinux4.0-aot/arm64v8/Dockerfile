ARG REPO=mcr.microsoft.com/dotnet/sdk
FROM $REPO:11.0.100-preview.5-azurelinux4.0-arm64v8

RUN dnf install -y \
        gcc \
    && dnf clean all
