ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:9.0.5-alpine3.21-amd64 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=9.0.5 \
    && wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
    && dotnet_sha512='81a26dd91953a474208b4b43cf4174442e9f9502437f257e66ecefe4f9b9ba3ccf5b3899daac061e3a0acc141d5be207c03e2b11136f4791b2911c3ef3e24889' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:9.0.5-alpine3.21-amd64

# .NET Runtime version
ENV DOTNET_VERSION=9.0.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
