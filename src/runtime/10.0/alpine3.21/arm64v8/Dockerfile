ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:10.0.0-preview.1-alpine3.21-arm64v8 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.1.25080.5 \
    && wget -O dotnet.tar.gz https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm64.tar.gz \
    && dotnet_sha512='2cc3f7715ef6d9986db73376bcbe7c6cdbb9f7a58b43e7be051c2ed457f7efc248a3c4dc87738467ad99a82ca385eea53ebc8af11c18900f5f980c8b76d9a475' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:10.0.0-preview.1-alpine3.21-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.1.25080.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
