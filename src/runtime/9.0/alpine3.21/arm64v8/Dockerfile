ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:9.0.4-alpine3.21-arm64v8 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=9.0.4 \
    && wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm64.tar.gz \
    && dotnet_sha512='f91cb422005c64c92c804b02670526492446cbe368633579dbf670a4a6d679c74ef012b8ee73fea672ccd85f0aa44c7164ed20876772af15052fa7447c10e255' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:9.0.4-alpine3.21-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=9.0.4

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
