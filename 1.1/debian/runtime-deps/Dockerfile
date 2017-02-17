FROM debian:jessie

# Metadata as defined at http://label-schema.org
ARG VCS_REF
ARG BUILD_DATE
LABEL org.label-schema.schema-version="1.0" \
      org.label-schema.vendor="Microsoft" \
      org.label-schema.name=".NET Core - Runtime Dependencies" \
      org.label-schema.license="MIT" \
      org.label-schema.description="This image contains the operating system with all of the native dependencies needed by .NET Core. This image DOES NOT include .NET Core and is designed for self-contained applications." \
      org.label-schema.url="https://docs.microsoft.com/en-us/dotnet/articles/core/" \
      org.label-schema.usage="https://github.com/dotnet/dotnet-docker" \
      org.label-schema.build-date=$BUILD_DATE \
      org.label-schema.vcs-url="https://github.com/dotnet/dotnet-docker.git" \
      org.label-schema.vcs-ref=$VCS_REF

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        ca-certificates \
        \
# .NET Core dependencies
        libc6 \
        libcurl3 \
        libgcc1 \
        libgssapi-krb5-2 \
        libicu52 \
        liblttng-ust0 \
        libssl1.0.0 \
        libstdc++6 \
        libunwind8 \
        libuuid1 \
        zlib1g \
    && rm -rf /var/lib/apt/lists/*
