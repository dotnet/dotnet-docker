ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:9.0.6-alpine3.22-arm64v8 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=9.0.6 \
    && wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm64.tar.gz \
    && dotnet_sha512='47900f24b161dcb27a6a0c4faccd372a40e0c0c5ebaae15faea1790ab3a23414f7eaff338142b34509c0bc61ec632952a07bc0bad5137d3871fc8553a66cffc5' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:9.0.6-alpine3.22-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=9.0.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
