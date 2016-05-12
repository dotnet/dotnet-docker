FROM buildpack-deps:jessie-scm

# Work around https://github.com/dotnet/cli/issues/1582 until Docker releases a
# fix (https://github.com/docker/docker/issues/20818). This workaround allows
# the container to be run with the default seccomp Docker settings by avoiding
# the restart_syscall made by LTTng which causes a failed assertion.
ENV LTTNG_UST_REGISTER_TIMEOUT 0

# Install .NET CLI dependencies
RUN echo "deb [arch=amd64] http://llvm.org/apt/jessie/ llvm-toolchain-jessie-3.6 main" > /etc/apt/sources.list.d/llvm.list \
    && wget -q -O - http://llvm.org/apt/llvm-snapshot.gpg.key|apt-key add - \
    && apt-get update \
    && apt-get install -y --no-install-recommends \
        clang-3.5 \
        libc6 \
        libcurl3 \
        libgcc1 \
        libicu52 \
        liblldb-3.6 \
        liblttng-ust0 \
        libssl1.0.0 \
        libstdc++6 \
        libtinfo5 \
        libunwind8 \
        libuuid1 \
        zlib1g \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core SDK
ENV DOTNET_CORE_SDK_VERSION 1.0.0-preview1-002702
RUN curl -SL https://dotnetcli.blob.core.windows.net/dotnet/beta/Binaries/$DOTNET_CORE_SDK_VERSION/dotnet-dev-debian-x64.$DOTNET_CORE_SDK_VERSION.tar.gz --output dotnet.tar.gz \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
