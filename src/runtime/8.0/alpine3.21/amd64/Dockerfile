ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:8.0.13-alpine3.21-amd64 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=8.0.13 \
    && wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
    && dotnet_sha512='fac7a79de9e79ca05583acb04c086b1a573ada0895b7c730176c181f0d39da82ac2ba5426681ded6d5b1d904e97c585082cc9b56d8c5207fee3e93b88b139505' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:8.0.13-alpine3.21-amd64

# .NET Runtime version
ENV DOTNET_VERSION=8.0.13

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
