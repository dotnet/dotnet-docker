ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:10.0.0-preview.3-alpine3.21-amd64 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.4.25227.102 \
    && wget -O dotnet.tar.gz https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
    && dotnet_sha512='21ba4d50a07c822d5047d14d2ced11f8b021c519b4667a7bc85545312c58bcb73c0016a9ab0bc52d54710b5af23b02822ec5a89b5f8ec343658d255520d9cd9e' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:10.0.0-preview.3-alpine3.21-amd64

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.4.25227.102

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
