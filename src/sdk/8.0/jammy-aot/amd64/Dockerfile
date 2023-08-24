# syntax=docker/dockerfile:1
ARG REPO=mcr.microsoft.com/dotnet/sdk
FROM $REPO:8.0.100-rc.1-jammy-amd64

RUN echo "deb [arch=arm64] http://ports.ubuntu.com/ubuntu-ports/ jammy main restricted" > /etc/apt/sources.list.d/arm64.list \
    && echo "deb [arch=arm64] http://ports.ubuntu.com/ubuntu-ports/ jammy-updates main restricted" >> /etc/apt/sources.list.d/arm64.list

RUN sed -i -e 's/deb http/deb [arch=amd64] http/g' /etc/apt/sources.list \
    && dpkg --add-architecture arm64 \
    && apt-get update \
    && apt-get install -y --no-install-recommends \
        clang \
        gcc-aarch64-linux-gnu \
        zlib1g-dev:arm64 \
        zlib1g-dev \
    && rm -rf /var/lib/apt/lists/*
