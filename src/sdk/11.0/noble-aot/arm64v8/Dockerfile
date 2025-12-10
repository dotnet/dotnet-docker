# syntax=docker/dockerfile:1
ARG REPO=mcr.microsoft.com/dotnet/sdk
FROM $REPO:11.0.100-alpha.1-noble-arm64v8

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        clang \
        llvm \
        zlib1g-dev \
    && rm -rf /var/lib/apt/lists/*
