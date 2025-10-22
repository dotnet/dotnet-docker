ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:10.0.0-alpine3.22-arm64v8 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0 \
    && dotnet_build_version=$dotnet_version-rtm.25521.107 \
    && wget \
        https://ci.dot.net/public/Runtime/$dotnet_build_version/dotnet-runtime-$dotnet_version-linux-musl-arm64.tar.gz \
        https://ci.dot.net/public-checksums/Runtime/$dotnet_build_version/dotnet-runtime-$dotnet_version-linux-musl-arm64.tar.gz.sha512 \
    && echo "$(cat dotnet-runtime-$dotnet_version-linux-musl-arm64.tar.gz.sha512)  dotnet-runtime-$dotnet_version-linux-musl-arm64.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet-runtime-$dotnet_version-linux-musl-arm64.tar.gz --directory /dotnet \
    && rm \
        dotnet-runtime-$dotnet_version-linux-musl-arm64.tar.gz \
        dotnet-runtime-$dotnet_version-linux-musl-arm64.tar.gz.sha512


# .NET runtime image
FROM $REPO:10.0.0-alpine3.22-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
