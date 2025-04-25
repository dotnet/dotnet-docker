ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:10.0.0-preview.3-alpine3.21-arm64v8 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.4.25224.106 \
    && wget -O dotnet.tar.gz https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm64.tar.gz \
    && dotnet_sha512='380cab0caddb350838f3e6a83e57c115240bb0005ab82748e768d2320fcd5c782505f6e19b17e39ca5a9a11dd3da530befebc9ff6704bf81b6907dce446c9542' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:10.0.0-preview.3-alpine3.21-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.4.25224.106

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
