ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:8.0.16-alpine3.21-amd64 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=8.0.16 \
    && wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
    && dotnet_sha512='ae77fffe1fc8e6b65a6908bc6f8b413b3dc15a89db1a425974ad7686bf307b068076f77079ba06224129a74209c10aaba7a418a5e3ddf4dbeef889c2fc4da23c' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:8.0.16-alpine3.21-amd64

# .NET Runtime version
ENV DOTNET_VERSION=8.0.16

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
