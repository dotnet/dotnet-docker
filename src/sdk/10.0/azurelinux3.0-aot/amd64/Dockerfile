# syntax=docker/dockerfile:1
ARG REPO=mcr.microsoft.com/dotnet/sdk
FROM $REPO:10.0.100-rc.2-azurelinux3.0-amd64

RUN tdnf install -y \
        build-essential \
        clang \
        zlib-devel \
    && tdnf clean all
