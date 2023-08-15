# syntax=docker/dockerfile:1
ARG REPO=mcr.microsoft.com/dotnet/sdk
FROM $REPO:8.0.100-rc.1-alpine3.18-arm32v7

RUN apk add --upgrade --no-cache \
        clang \
        zlib-dev
