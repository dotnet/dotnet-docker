ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:8.0.14-alpine3.21-amd64 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=8.0.14 \
    && wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
    && dotnet_sha512='f9ddf59984ea9692a624ca1e7af2783693c564979eaf460dd4fbb3b72070faada1ee36a20895c492c886f061abf0dbb8327b1f8e0581cbe4991666f092b09789' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:8.0.14-alpine3.21-amd64

# .NET Runtime version
ENV DOTNET_VERSION=8.0.14

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
