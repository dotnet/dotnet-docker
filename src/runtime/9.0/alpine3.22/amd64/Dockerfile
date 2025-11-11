ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:9.0.11-alpine3.22-amd64 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=9.0.11 \
    && wget \
        https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
        https://builds.dotnet.microsoft.com/dotnet/checksums/$dotnet_version-sha.txt \
    && awk -v file="dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz" '{gsub(/\r/, "")} $2 == file' $dotnet_version-sha.txt | sha512sum -c \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz --directory /dotnet \
    && rm \
        dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
        $dotnet_version-sha.txt


# .NET runtime image
FROM $REPO:9.0.11-alpine3.22-amd64

# .NET Runtime version
ENV DOTNET_VERSION=9.0.11

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
