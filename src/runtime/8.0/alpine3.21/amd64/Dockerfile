ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:8.0.15-alpine3.21-amd64 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=8.0.15 \
    && wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
    && dotnet_sha512='424fb3f807c809271752f79189abee1e6cf9f2326e1cbb5866cbc030d6b6deae98a3913fb5ecb5f944f8c13869fa4c53baa19ddeba1c2b07f9ae72739f061a74' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:8.0.15-alpine3.21-amd64

# .NET Runtime version
ENV DOTNET_VERSION=8.0.15

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
