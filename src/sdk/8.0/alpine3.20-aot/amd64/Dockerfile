# syntax=docker/dockerfile:1
ARG REPO=mcr.microsoft.com/dotnet/sdk
FROM $REPO:8.0.302-alpine3.20-amd64

RUN apk add --upgrade --no-cache \
        build-base \
        clang \
        zlib-dev
