ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:8.0.14-alpine3.20-arm64v8 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=8.0.14 \
    && wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm64.tar.gz \
    && dotnet_sha512='b428b6d54414af5147bb6a806f978945540b1adb36a5d432d94dedeb798e2cffa279cdf327a5464357743b526700487aef7e6b118ceecd9d4b623400b04ae5db' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:8.0.14-alpine3.20-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=8.0.14

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
