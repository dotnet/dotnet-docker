ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:8.0.16-alpine3.22-arm32v7 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=8.0.16 \
    && wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm.tar.gz \
    && dotnet_sha512='a1b43d5ca2315fda8d3777d20aaa9e4725a6867b0430d931069e6530dec32e6c6ef64132e7178828ed10c5a369f8c0c2b7a426012b75ee617eb3cf65305adb28' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:8.0.16-alpine3.22-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=8.0.16

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
