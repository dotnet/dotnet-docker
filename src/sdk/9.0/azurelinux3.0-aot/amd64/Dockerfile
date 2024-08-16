# syntax=docker/dockerfile:1
ARG REPO=mcr.microsoft.com/dotnet/sdk
FROM $REPO:9.0.100-rc.1-azurelinux3.0-amd64

RUN tdnf install -y \
        build-essential \
        clang \
        zlib-devel \
    && tdnf clean all
