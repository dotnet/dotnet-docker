ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:10.0.0-rc.2-alpine3.22-amd64 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-rc.2.25479.113 \
    && wget \
        https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
        https://ci.dot.net/public-checksums/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz.sha512 \
    && echo "$(cat dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz.sha512)  dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz --directory /dotnet \
    && rm \
        dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
        dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz.sha512


# .NET runtime image
FROM $REPO:10.0.0-rc.2-alpine3.22-amd64

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-rc.2.25479.113

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
