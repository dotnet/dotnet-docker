ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:9.0.0-azurelinux3.0-arm64v8 AS installer

RUN tdnf install -y \
        tar \
    && tdnf clean all

# Retrieve .NET Runtime
RUN dotnet_version=9.0.0-rtm.24516.10 \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-9.0.0-linux-arm64.tar.gz \
    && dotnet_sha512='688169a1384ba39d6a6fec2b03cf9c9a03c691dc5588f918bcaebadf496d6f5d19b53fcbe9e739d0c8950ba7b022ebc0026b0a2c1c2a527b9a3ee7d904f5dbbf' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:9.0.0-azurelinux3.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=9.0.0-rtm.24516.10

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
