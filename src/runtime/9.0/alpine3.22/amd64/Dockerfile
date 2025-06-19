ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:9.0.6-alpine3.22-amd64 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=9.0.6 \
    && wget --output-document dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
    && dotnet_sha512='70ae92ffc4cd1cf074468d8671a6b97ce44debccaf724bcb517af655cd2e75d7f9876389e3d83ca4e745df51e97c2b2209b739ea29cb24dd68b34db12f58827d' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum --check - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet.tar.gz --directory /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:9.0.6-alpine3.22-amd64

# .NET Runtime version
ENV DOTNET_VERSION=9.0.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln --symbolic /usr/share/dotnet/dotnet /usr/bin/dotnet
