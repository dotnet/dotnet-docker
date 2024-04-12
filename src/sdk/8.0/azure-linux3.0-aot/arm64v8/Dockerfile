# syntax=docker/dockerfile:1
ARG REPO=mcr.microsoft.com/dotnet/sdk
FROM $REPO:8.0.204-azure-linux3.0-arm64v8

RUN tdnf install -y \
        build-essential \
        clang \
        zlib-devel \
    && tdnf clean all
