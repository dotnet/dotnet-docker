ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:8.0.0-preview.1-cbl-mariner2.0-arm64v8 AS installer

RUN tdnf install -y \
        tar \
    && tdnf clean all

# Retrieve .NET Runtime
RUN dotnet_version=8.0.0-preview.1.23081.6 \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm64.tar.gz \
    && dotnet_sha512='cbc8ffd352181b198e7feb7eafd110d027b514676e9b34dbd4b5bf288ce4c9dce3b87c5afaf682f84ec1ef081472cca4c5f35d43c7262db5a8745bcfcee90070' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:8.0.0-preview.1-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=8.0.0-preview.1.23081.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
