ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:9.0.5-alpine3.21-arm32v7 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=9.0.5 \
    && wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm.tar.gz \
    && dotnet_sha512='cce09a24c0b9870e329389ab2aae2e76ec22bb7838654850ab17b56951fba00b15ca0692e363cd5e1e5d10d1e1df5a68547d3c2161436b857078d0182befd22a' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:9.0.5-alpine3.21-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=9.0.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
