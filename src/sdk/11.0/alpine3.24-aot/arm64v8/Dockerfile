ARG REPO=mcr.microsoft.com/dotnet/sdk
FROM $REPO:11.0.100-preview.6-alpine3.24-arm64v8

RUN apk add --upgrade --no-cache \
        gcc \
        musl-dev \
        zlib-dev
