ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM cblmariner.azurecr.io/base/core:1.0 AS installer

RUN tdnf install -y \
        ca-certificates-microsoft \
    && tdnf clean all

# Retrieve .NET Runtime
RUN dotnet_version=7.0.0-preview.2.22116.9 \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='d9ff46dac6052b8278f1ae8fc007761f71aa4d55c3dc19ffe93c73e1a9204019c6f39d6ff4db1af84083b5495cf3c7d51e2bcfef588ae505a2526e7bdcd65c75' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# .NET runtime image
FROM $REPO:7.0.0-preview.2-cbl-mariner1.0-distroless-amd64

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-preview.2.22116.9

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]
