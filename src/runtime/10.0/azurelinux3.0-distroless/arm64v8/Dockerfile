ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-rc.1 \
    && dotnet_build_version=$dotnet_version.25416.112 \
    && curl --fail --show-error --location \
        --remote-name https://ci.dot.net/public/Runtime/$dotnet_build_version/dotnet-runtime-$dotnet_version-linux-arm64.tar.gz \
        --remote-name https://ci.dot.net/public-checksums/Runtime/$dotnet_build_version/dotnet-runtime-$dotnet_version-linux-arm64.tar.gz.sha512 \
    && echo "$(cat dotnet-runtime-$dotnet_version-linux-arm64.tar.gz.sha512)  dotnet-runtime-$dotnet_version-linux-arm64.tar.gz" | sha512sum -c - \
    && mkdir --parents /usr/share/dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet-runtime-$dotnet_version-linux-arm64.tar.gz --directory /usr/share/dotnet \
    && rm \
        dotnet-runtime-$dotnet_version-linux-arm64.tar.gz \
        dotnet-runtime-$dotnet_version-linux-arm64.tar.gz.sha512

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# .NET runtime image
FROM $REPO:10.0.0-rc.1-azurelinux3.0-distroless-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-rc.1.25416.112

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
