# syntax=docker/dockerfile:1
ARG REPO=mcr.microsoft.com/dotnet/sdk
FROM $REPO:11.0.100-preview.1-alpine3.22-amd64

RUN apk add --upgrade --no-cache \
        build-base \
        clang \
        zlib-dev
