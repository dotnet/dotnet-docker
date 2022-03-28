ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:focal-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=7.0.0-preview.3.22173.5 \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm.tar.gz \
    && dotnet_sha512='5f18ae6c18c5bd4ce2d1f6e5aa6a7bf8d715e8f2687ef314878ba5f2f20ba91867f71f5d153f11bbb7aee16fbf97ccfeb6c16968251e32ef0f833995b1fbdfd9' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-preview.3-focal-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-preview.3.22173.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
