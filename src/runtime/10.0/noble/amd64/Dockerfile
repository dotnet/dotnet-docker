ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:noble-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-rc.1 \
    && curl --fail --show-error --location \
        --remote-name https://ci.dot.net/public/Runtime/$dotnet_version.25416.112/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
        --remote-name https://ci.dot.net/public-checksums/Runtime/$dotnet_version.25416.112/dotnet-runtime-$dotnet_version-linux-x64.tar.gz.sha512 \
    && echo "$(cat dotnet-runtime-$dotnet_version-linux-x64.tar.gz.sha512)  dotnet-runtime-$dotnet_version-linux-x64.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet-runtime-$dotnet_version-linux-x64.tar.gz --directory /dotnet \
    && rm \
        dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
        dotnet-runtime-$dotnet_version-linux-x64.tar.gz.sha512


# .NET runtime image
FROM $REPO:10.0.0-rc.1-noble-amd64

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-rc.1.25416.112

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
