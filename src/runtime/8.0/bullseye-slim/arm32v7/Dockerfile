ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=8.0.0-alpha.1.23053.7 \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm.tar.gz \
    && dotnet_sha512='cfb217c45294d2aa6bf450daebff93c7f0ac47944e9b5ab69c0871be37c34cce11c2a7fdee33bf5b7e7ed9bfcfe4cca602a8b5462e62fdcc923dd7fd6863395b' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:8.0.0-alpha.1-bullseye-slim-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=8.0.0-alpha.1.23053.7

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
