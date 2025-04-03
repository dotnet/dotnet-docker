ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:9.0.4-alpine3.20-arm32v7 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=9.0.4 \
    && wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm.tar.gz \
    && dotnet_sha512='152315bad4db61f6de20e47103e0804f64e9dbc3adbb8cf874c3e979a0d19b4b26f5fac07f1cd224a8881f4afdedb3c244f970a7d647eae1aa44ac2604c43526' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:9.0.4-alpine3.20-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=9.0.4

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
