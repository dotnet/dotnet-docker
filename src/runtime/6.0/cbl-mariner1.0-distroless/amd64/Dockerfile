ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM cblmariner.azurecr.io/base/core:1.0 AS installer

RUN tdnf install -y \
        ca-certificates-microsoft \
    && tdnf clean all

# Retrieve .NET Runtime
RUN dotnet_version=6.0.3 \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='083d9e6e72f0d8f175b341f5229277374e630c5358cfd3602fe611aeef59abec715edbe18d62135a5d13a650e99ef49f19b17e8c81663d0b5bee757519bec894' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# .NET runtime image
FROM $REPO:6.0.3-cbl-mariner1.0-distroless-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.3

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]
