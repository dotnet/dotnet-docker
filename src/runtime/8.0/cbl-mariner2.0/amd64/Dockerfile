ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:8.0.0-alpha.1-cbl-mariner2.0-amd64 AS installer

RUN tdnf install -y \
        tar \
    && tdnf clean all

# Retrieve .NET Runtime
RUN dotnet_version=8.0.0-alpha.1.23053.7 \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='5221ecdb0cf75613d976599693aaf2ae806e3a506e40e2b6c75e1207e5b70735a191c7c3090fd88193e126937c1cdb731072ed5486079986d8f01d070841e098' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:8.0.0-alpha.1-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=8.0.0-alpha.1.23053.7

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
