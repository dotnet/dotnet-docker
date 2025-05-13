ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:noble-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.4.25258.110 \
    && curl -fSL --output dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='1902b230b1f200fec7762a14b1c7e140172a155bd8312d5338ea290c9507a61be3ed7dc7995e5769e9e0f55b11dfe2ee7585a654e6f4b4464e59415463ce7ade' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:10.0.0-preview.4-noble-amd64

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.4.25258.110

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
