# syntax=docker/dockerfile:1
ARG REPO=mcr.microsoft.com/dotnet/sdk
FROM $REPO:8.0.100-rc.1-cbl-mariner2.0-amd64

RUN tdnf install -y \
        build-essential \
        clang \
        zlib-devel \
    && tdnf clean all
