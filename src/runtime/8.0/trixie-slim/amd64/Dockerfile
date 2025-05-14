ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:trixie-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=8.0.16 \
    && curl -fSL --output dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='e0924f88b6795d669e5c6278845a574b75fbd9b5aeb63a0ed638224fb8d3d48b91bb2a9afb272dfd5f40e8241d41a84792b9fcfe426e8fdcfa14116bc132cee2' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:8.0.16-trixie-slim-amd64

# .NET Runtime version
ENV DOTNET_VERSION=8.0.16

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
