ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:10.0.0-preview.3-alpine3.21-arm32v7 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.3.25169.5 \
    && wget -O dotnet.tar.gz https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm.tar.gz \
    && dotnet_sha512='3c832f207bd50a8e17bbe1f14add66c4fc213cccdc13183e12966028b5731ce853b15265ef609b898f4eeeb9afebc5771f5b1f9140f20504ae28bdb417186498' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:10.0.0-preview.3-alpine3.21-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.3.25169.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
