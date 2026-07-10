ARG REPO=mcr.microsoft.com/dotnet/sdk
FROM $REPO:11.0.100-preview.6-resolute-amd64

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        gcc \
        zlib1g-dev \
    && rm -rf /var/lib/apt/lists/*
