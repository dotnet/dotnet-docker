ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:resolute-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=11.0.0-alpha.1.25609.108 \
    && curl --fail --show-error --location \
        --remote-name https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm.tar.gz \
        --remote-name https://ci.dot.net/public-checksums/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm.tar.gz.sha512 \
    && echo "$(cat dotnet-runtime-$dotnet_version-linux-arm.tar.gz.sha512)  dotnet-runtime-$dotnet_version-linux-arm.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet-runtime-$dotnet_version-linux-arm.tar.gz --directory /dotnet \
    && rm \
        dotnet-runtime-$dotnet_version-linux-arm.tar.gz \
        dotnet-runtime-$dotnet_version-linux-arm.tar.gz.sha512


# .NET runtime image
FROM $REPO:11.0.0-alpha.1-resolute-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=11.0.0-alpha.1.25609.108

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
