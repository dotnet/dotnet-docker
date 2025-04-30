ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:10.0.0-preview.3-alpine3.21-arm64v8 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.4.25229.110 \
    && wget -O dotnet.tar.gz https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm64.tar.gz \
    && dotnet_sha512='fb1915d377d0ac50a497c37bc3eb03f1c1fde8a8677659007eebc7ccd3b57082cec17cb3f3c790692d764b03f04913aaa73f4abcddc39b3709bed4f3022b8af6' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:10.0.0-preview.3-alpine3.21-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.4.25229.110

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
