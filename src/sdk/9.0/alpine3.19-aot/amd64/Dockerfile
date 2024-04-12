# syntax=docker/dockerfile:1
ARG REPO=mcr.microsoft.com/dotnet/sdk
FROM $REPO:9.0.100-preview.4-alpine3.19-amd64

RUN apk add --upgrade --no-cache \
        build-base \
        clang \
        zlib-dev
