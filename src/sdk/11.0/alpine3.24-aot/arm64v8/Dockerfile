ARG REPO=mcr.microsoft.com/dotnet/sdk
FROM $REPO:11.0.100-preview.5-alpine3.24-arm64v8

RUN apk add --upgrade --no-cache \
        build-base \
        clang \
        zlib-dev
