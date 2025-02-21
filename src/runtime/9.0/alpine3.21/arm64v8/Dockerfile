ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:9.0.2-alpine3.21-arm64v8 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=9.0.2 \
    && wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm64.tar.gz \
    && dotnet_sha512='0bb0dc7a4388c5b95d4fec9fe7ca1273f9afa502021c73eb946ed2928d6d6d0836414400f667eff30da97b83b04f66d3032c2344bb2587007b4001d75b696a16' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:9.0.2-alpine3.21-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=9.0.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
