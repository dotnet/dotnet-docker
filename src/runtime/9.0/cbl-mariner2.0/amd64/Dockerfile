ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:9.0.0-preview.2-cbl-mariner2.0-amd64 AS installer

RUN tdnf install -y \
        tar \
    && tdnf clean all

# Retrieve .NET Runtime
RUN dotnet_version=9.0.0-preview.2.24128.5 \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='6433959a75103f2f1108bbc16cfe348f9ba04fec1c8f9b6895019241bfcb7b21fab675cc13971f2c1a66b46b044a95f91e1e2b46e6e8bdd893d277906f82545a' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:9.0.0-preview.2-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=9.0.0-preview.2.24128.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
