ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:9.0.11-azurelinux3.0-amd64 AS installer

RUN tdnf install -y \
        tar \
    && tdnf clean all

# Retrieve .NET Runtime
RUN dotnet_version=9.0.11 \
    && curl --fail --show-error --location \
        --remote-name https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
        --remote-name https://builds.dotnet.microsoft.com/dotnet/checksums/$dotnet_version-sha.txt \
    && sed -i 's/\r$//' $dotnet_version-sha.txt \
    && sha512sum -c $dotnet_version-sha.txt --ignore-missing \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet-runtime-$dotnet_version-linux-x64.tar.gz --directory /dotnet \
    && rm \
        dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
        $dotnet_version-sha.txt


# .NET runtime image
FROM $REPO:9.0.11-azurelinux3.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=9.0.11

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
