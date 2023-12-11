# syntax=docker/dockerfile:1
ARG REPO=mcr.microsoft.com/dotnet/sdk
FROM $REPO:9.0.100-alpha.1-alpine3.18-arm64v8

RUN apk add --upgrade --no-cache \
        build-base \
        clang \
        zlib-dev
