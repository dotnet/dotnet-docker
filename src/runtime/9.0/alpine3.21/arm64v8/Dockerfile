ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:9.0.3-alpine3.21-arm64v8 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=9.0.3 \
    && wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm64.tar.gz \
    && dotnet_sha512='2385114169e32b29b3a13b275a441af1edbb4c14ebed52ff8f45f11ac8a571efa2984be2ef91e38e670038c8e29db8c585ff12d1e05502f4120e4dd05e98b72c' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:9.0.3-alpine3.21-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=9.0.3

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
