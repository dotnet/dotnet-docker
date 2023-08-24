# syntax=docker/dockerfile:1
ARG REPO=mcr.microsoft.com/dotnet/sdk
FROM $REPO:8.0.100-rc.1-jammy-arm64v8

RUN echo "deb [arch=amd64] http://archive.ubuntu.com/ubuntu/ jammy main restricted" > /etc/apt/sources.list.d/amd64.list \
    && echo "deb [arch=amd64] http://archive.ubuntu.com/ubuntu/ jammy-updates main restricted" >> /etc/apt/sources.list.d/amd64.list

RUN sed -i -e 's/deb http/deb [arch=arm64] http/g' /etc/apt/sources.list \
    && dpkg --add-architecture amd64 \
    && apt-get update \
    && apt-get install -y --no-install-recommends \
        clang \
        gcc-x86-64-linux-gnu \
        zlib1g-dev:amd64 \
        zlib1g-dev \
    && rm -rf /var/lib/apt/lists/*
