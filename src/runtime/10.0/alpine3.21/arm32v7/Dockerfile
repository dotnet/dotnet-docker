ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:10.0.0-preview.3-alpine3.21-arm32v7 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.4.25227.102 \
    && wget -O dotnet.tar.gz https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm.tar.gz \
    && dotnet_sha512='9f07cba4e1c055bdfd6c44ad358e5d5e510754a3b5e14ea54f2bd77af0456d3c17195b7a97655eab3b22e71e54c9972320c11a0527c7d5a25c8741f887cbc36b' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:10.0.0-preview.3-alpine3.21-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.4.25227.102

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
