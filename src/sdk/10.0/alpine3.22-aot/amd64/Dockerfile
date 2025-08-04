# syntax=docker/dockerfile:1
ARG REPO=mcr.microsoft.com/dotnet/sdk
FROM $REPO:10.0.100-preview.7-alpine3.22-amd64

RUN apk add --upgrade --no-cache \
        build-base \
        clang \
        zlib-dev
