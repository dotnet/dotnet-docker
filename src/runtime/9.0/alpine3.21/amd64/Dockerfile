ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:9.0.2-alpine3.21-amd64 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=9.0.2 \
    && wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
    && dotnet_sha512='df116ef9b7f6b717b7c7f057e826c9e1f1ed0d743fa6b26e9229fc36e500ab834d19ae1ab55ebc28b1c9b8cb4a7f41c62edd08c2dd2cdcb6e912defea2810ffb' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:9.0.2-alpine3.21-amd64

# .NET Runtime version
ENV DOTNET_VERSION=9.0.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
