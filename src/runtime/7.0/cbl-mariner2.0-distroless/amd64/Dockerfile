ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM mcr.microsoft.com/cbl-mariner/base/core:2.0 AS installer

RUN tdnf install -y \
        ca-certificates-microsoft \
        gzip \
        tar \
    && tdnf clean all

# Retrieve .NET Runtime
RUN dotnet_version=7.0.0-preview.5.22272.3 \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='930f8195715c4b0c1cc0696b1c7bf1d4c87eaa385dfc9d4bef1a175afd709f0dee9af0925bccf6716fd427a19869fe09177ce633b0f3052a093a94e294dd3c07' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# .NET runtime image
FROM $REPO:7.0.0-preview.5-cbl-mariner2.0-distroless-amd64

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-preview.5.22272.3

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]
