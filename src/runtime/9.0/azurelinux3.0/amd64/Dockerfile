ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:9.0.6-azurelinux3.0-amd64 AS installer

RUN tdnf install --assumeyes \
        tar \
    && tdnf clean all

# Retrieve .NET Runtime
RUN dotnet_version=9.0.6 \
    && curl --fail --show-error --location --output dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='bfe257b7a6dc05053a784d3ac966ca7fdb7a2efd5538e27c3ea822aabc9d3a20f44af4486df10021a5f64c893224c2d947efd06492d1df3705556df76737126f' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum --check - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet.tar.gz --directory /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:9.0.6-azurelinux3.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=9.0.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln --symbolic /usr/share/dotnet/dotnet /usr/bin/dotnet
