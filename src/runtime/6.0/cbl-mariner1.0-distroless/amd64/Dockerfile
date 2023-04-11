ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM mcr.microsoft.com/cbl-mariner/base/core:1.0 AS installer

RUN tdnf install -y \
        ca-certificates \
    && tdnf clean all

# Retrieve .NET Runtime
RUN dotnet_version=6.0.16 \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='c8891b791a51e7d2c3164470dfd2af2ce59af3c26404e84075277e307df7dcd1e3ccf1a1a3c2655fe2eea8a30f8349b7adbbe5de4cedfee52da06729a505d8f5' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# .NET runtime image
FROM $REPO:6.0.16-cbl-mariner1.0-distroless-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.16

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]
