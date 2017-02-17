FROM microsoft/dotnet:1.0-runtime-deps

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core
ENV DOTNET_VERSION 1.0.3
ENV DOTNET_DOWNLOAD_URL https://dotnetcli.blob.core.windows.net/dotnet/preview/Binaries/$DOTNET_VERSION/dotnet-debian-x64.$DOTNET_VERSION.tar.gz

# Metadata as defined at http://label-schema.org
ARG VCS_REF
ARG BUILD_DATE
LABEL org.label-schema.schema-version="1.0" \
      org.label-schema.vendor="Microsoft" \
      org.label-schema.name=".NET Core - Runtime" \
      org.label-schema.version=$DOTNET_VERSION \
      org.label-schema.license="MIT" \
      org.label-schema.description="This image contains the .NET Core (runtime and libraries) and is optimized for running .NET Core apps in production." \
      org.label-schema.url="https://docs.microsoft.com/en-us/dotnet/articles/core/" \
      org.label-schema.usage="https://github.com/dotnet/dotnet-docker" \
      org.label-schema.build-date=$BUILD_DATE \
      org.label-schema.vcs-url="https://github.com/dotnet/dotnet-docker.git" \
      org.label-schema.vcs-ref=$VCS_REF

RUN curl -SL $DOTNET_DOWNLOAD_URL --output dotnet.tar.gz \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
