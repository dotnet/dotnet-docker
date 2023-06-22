ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=8.0.0-preview.6.23321.2 \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm.tar.gz \
    && dotnet_sha512='570c027b5e326b45a5dc56629255ed3c5f97ca1c5cb46be743c56f8e201996064e523d12676ed643a7a0724602bfb486f036318f1cb5ee49c62e55ea825fe4f6' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:8.0.0-preview.6-jammy-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=8.0.0-preview.6.23321.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
