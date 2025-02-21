ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:8.0.13-alpine3.20-arm64v8 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=8.0.13 \
    && wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm64.tar.gz \
    && dotnet_sha512='7b469196495ed00d9cfb166558704c22ac572be11fc4df936d86912d9c306b101ba3411b68e95c8d199f86ba48caa42d9e2b068ceae27b47347dc50b7b96ad18' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:8.0.13-alpine3.20-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=8.0.13

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
