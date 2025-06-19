ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:9.0.6-alpine3.21-arm32v7 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=9.0.6 \
    && wget --output-document dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm.tar.gz \
    && dotnet_sha512='a7979a10bf8124f83510b68d9f5036bd92a21e3b625fe334cc971b79ce2168bde265ef76c92d5f909e2539fd443ad7f14265270647c98770717fc666ad15bd9e' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum --check - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet.tar.gz --directory /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:9.0.6-alpine3.21-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=9.0.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln --symbolic /usr/share/dotnet/dotnet /usr/bin/dotnet
