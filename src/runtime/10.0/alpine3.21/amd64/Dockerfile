ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:10.0.0-preview.3-alpine3.21-amd64 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.4.25255.103 \
    && wget -O dotnet.tar.gz https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
    && dotnet_sha512='a5d6e35a29b24583952c608478e48997a00258966dfa4a7abb491228133951a479de04c86cd69f99b7d892dc3456a512c828a0d5cd83eefeb9076d4785e603eb' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:10.0.0-preview.3-alpine3.21-amd64

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.4.25255.103

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
