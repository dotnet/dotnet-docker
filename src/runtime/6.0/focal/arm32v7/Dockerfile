ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
ARG DOTNET_VERSION=6.0.0-preview.3.21167.1

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer
ARG DOTNET_VERSION

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-arm.tar.gz \
    && dotnet_sha512='91953dd52f5e259db93d7612ffb4e8afd1c5a52c8e2b359dd2d2950a0e6086a61ab8d002caba26b39202939cab81893bf4f63c8a25dd49942f81bd28b876ed7a' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0-focal-arm32v7
ARG DOTNET_VERSION

ENV DOTNET_VERSION=$DOTNET_VERSION

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
