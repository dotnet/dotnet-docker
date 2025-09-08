ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:10.0.0-rc.1-alpine3.22-arm32v7 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-rc.1 \
    && wget \
        https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version.25451.107/dotnet-runtime-$dotnet_version-linux-musl-arm.tar.gz \
        https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version.25451.107/dotnet-runtime-$dotnet_version-linux-musl-arm.tar.gz.sha512 \
    && sha512sum -c dotnet-runtime-$dotnet_version-linux-musl-arm.tar.gz.sha512 \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet-runtime-$dotnet_version-linux-musl-arm.tar.gz --directory /dotnet \
    && rm \
        dotnet-runtime-$dotnet_version-linux-musl-arm.tar.gz \
        dotnet-runtime-$dotnet_version-linux-musl-arm.tar.gz.sha512


# .NET runtime image
FROM $REPO:10.0.0-rc.1-alpine3.22-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-rc.1.25451.107

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
