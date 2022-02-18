ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:focal-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=7.0.0-preview.2.22116.9 \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm.tar.gz \
    && dotnet_sha512='5081004f48d9738ccd9a4fc90580dc6ee2927b0c00696e501f27cd806a541e52184ca61c6f9b4f65b59051035e83741e3644e34f42b7a2e79d5c165db814a4e1' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-preview.2-focal-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-preview.2.22116.9

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
