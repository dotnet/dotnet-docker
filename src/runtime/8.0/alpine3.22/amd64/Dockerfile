ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:8.0.17-alpine3.22-amd64 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=8.0.17 \
    && wget \
        https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
        https://builds.dotnet.microsoft.com/dotnet/checksums/8.0.17-sha.txt \
    && awk -v file="dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz" '{gsub(/\r/, "")} $2 == file' 8.0.17-sha.txt | sha512sum -c \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz --directory /dotnet \
    && rm \
        dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
        8.0.17-sha.txt


# .NET runtime image
FROM $REPO:8.0.17-alpine3.22-amd64

# .NET Runtime version
ENV DOTNET_VERSION=8.0.17

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
