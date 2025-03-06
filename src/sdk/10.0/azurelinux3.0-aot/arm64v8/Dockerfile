# syntax=docker/dockerfile:1
ARG REPO=mcr.microsoft.com/dotnet/sdk
FROM $REPO:10.0.100-preview.1-azurelinux3.0-arm64v8

RUN tdnf install -y \
        build-essential \
        clang \
        zlib-devel \
    && tdnf clean all
