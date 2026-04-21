ARG REPO=mcr.microsoft.com/dotnet/sdk
FROM $REPO:10.0.203-alpine3.23-arm64v8

RUN apk add --upgrade --no-cache \
        build-base \
        clang \
        zlib-dev
