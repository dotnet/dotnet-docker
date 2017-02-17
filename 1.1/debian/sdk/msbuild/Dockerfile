FROM buildpack-deps:jessie-scm

# Install .NET CLI dependencies
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
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

# Install .NET Core SDK
ENV DOTNET_VERSION 1.1.0
ENV DOTNET_SDK_VERSION 1.0.0-rc4-004771
ENV DOTNET_SDK_DOWNLOAD_URL https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-dev-debian-x64.$DOTNET_SDK_VERSION.tar.gz

# Metadata as defined at http://label-schema.org
ARG VCS_REF
ARG BUILD_DATE
LABEL org.label-schema.schema-version="1.0" \
      org.label-schema.vendor="Microsoft" \
      org.label-schema.name=".NET Core - MSBuild based SDK" \
      org.label-schema.version="{ \"dotnet-core\":\"$DOTNET_VERSION\", \"dotnet-tools\": \"$DOTNET_SDK_VERSION\" }" \
      org.label-schema.license="MIT" \
      org.label-schema.description="This image contains the .NET Core SDK which is comprised of two parts: .NET Core and the .NET Core command line tools. Use this image for your MSBuild based development process (developing, building and testing applications). Once the tooling stabilizes, the project.json variant will be deprecated and there will only be the MSBuild variant." \
      org.label-schema.url="https://docs.microsoft.com/en-us/dotnet/articles/core/" \
      org.label-schema.usage="https://github.com/dotnet/dotnet-docker" \
      org.label-schema.build-date=$BUILD_DATE \
      org.label-schema.vcs-url="https://github.com/dotnet/dotnet-docker.git" \
      org.label-schema.vcs-ref=$VCS_REF

RUN curl -SL $DOTNET_SDK_DOWNLOAD_URL --output dotnet.tar.gz \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet

# Trigger the population of the local package cache
ENV NUGET_XMLDOC_MODE skip
RUN mkdir warmup \
    && cd warmup \
    && dotnet new \
    && cd .. \
    && rm -rf warmup \
    && rm -rf /tmp/NuGetScratch
