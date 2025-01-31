ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:noble-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.1.25078.5 \
    && curl -fSL --output dotnet.tar.gz https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='38459ae6b6bca926c6cd410063b9f67d85fdba175c0d0a2ab6f56ffc84510234e6df69f96fa1bc1e32be6ed6289171f26eaee8acd462d978c7d01d5337c4caab' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:10.0.0-preview.1-noble-amd64

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.1.25078.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
