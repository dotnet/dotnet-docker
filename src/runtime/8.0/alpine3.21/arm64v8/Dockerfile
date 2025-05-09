ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:8.0.16-alpine3.21-arm64v8 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=8.0.16 \
    && wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm64.tar.gz \
    && dotnet_sha512='9af2ca767cd94e23b8e9165582abad18e11d3f25c972fabad19b97435144774aa427746e6056132fa2361e237815ce35e8b6244641d92f511427223565abb921' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:8.0.16-alpine3.21-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=8.0.16

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
