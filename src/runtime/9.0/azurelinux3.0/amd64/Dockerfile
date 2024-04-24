ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:9.0.0-preview.4-azurelinux3.0-amd64 AS installer

RUN tdnf install -y \
        tar \
    && tdnf clean all

# Retrieve .NET Runtime
RUN dotnet_version=9.0.0-preview.4.24223.11 \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='bf350edaecea17e445c1782a8164c35d9add3a520e719cfb0d8ba7250bc7ffe85b9be8a4a75ab75f9b90d00b7731014f0ef8048f6db35e30577e002ae9f03e53' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:9.0.0-preview.4-azurelinux3.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=9.0.0-preview.4.24223.11

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
