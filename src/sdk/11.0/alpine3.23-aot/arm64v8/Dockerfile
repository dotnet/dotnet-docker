ARG REPO=mcr.microsoft.com/dotnet/sdk
FROM $REPO:11.0.100-preview.4-alpine3.23-arm64v8

RUN apk add --upgrade --no-cache \
        build-base \
        clang \
        zlib-dev
