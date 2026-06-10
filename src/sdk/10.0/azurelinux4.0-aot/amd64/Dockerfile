ARG REPO=mcr.microsoft.com/dotnet/sdk
FROM $REPO:10.0.301-azurelinux4.0-amd64

RUN dnf install -y \
        gcc \
    && dnf clean all
