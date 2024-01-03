# syntax=docker/dockerfile:1
ARG REPO=mcr.microsoft.com/dotnet/sdk
FROM $REPO:8.0.100-alpine3.19-amd64

RUN apk add --upgrade --no-cache \
        build-base \
        clang \
        zlib-dev
