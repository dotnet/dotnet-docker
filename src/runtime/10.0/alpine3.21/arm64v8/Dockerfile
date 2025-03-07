ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:10.0.0-preview.2-alpine3.21-arm64v8 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.2.25156.1 \
    && wget -O dotnet.tar.gz https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm64.tar.gz \
    && dotnet_sha512='431ff665cfcb260bc34ed2b5a376af4d8b5c9eba5d2d2568c35dd9f06af7fd22c000203b5ff9bf66fc2d977b9f56ebaf61aeb6a451ec6a4739623eaecabeaee7' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:10.0.0-preview.2-alpine3.21-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.2.25156.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
