ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:10.0.0-preview.3-alpine3.21-arm64v8 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.3.25171.5 \
    && wget -O dotnet.tar.gz https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm64.tar.gz \
    && dotnet_sha512='7532ab4d168ad7ca86fdca46db3f9447b9ece86688fdc011d4597e4a68aedaa4b3af0079c2f92f4ab2a546e7ab6ecf525dc69ef4c11d032c84c29738c7a62ada' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:10.0.0-preview.3-alpine3.21-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.3.25171.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
