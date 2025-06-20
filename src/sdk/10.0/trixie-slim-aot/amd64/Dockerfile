# syntax=docker/dockerfile:1
ARG REPO=mcr.microsoft.com/dotnet/sdk
FROM $REPO:10.0.100-preview.6-trixie-slim-amd64

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        clang \
        llvm \
        zlib1g-dev \
    && rm --recursive --force /var/lib/apt/lists/*
