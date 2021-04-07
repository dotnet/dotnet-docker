ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
ARG DOTNET_VERSION=6.0.0-preview.3.21201.4

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl as installer
ARG DOTNET_VERSION

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-arm.tar.gz \
    && dotnet_sha512='02ede5efac794b181719642771156d5b9759f52d90fe7c05edc5e5ec6f9ee22fa03ebebce29f4aef0c74399d468429ac335cae2f636c87566f33d8c04be2604e' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0-bullseye-slim-arm32v7
ARG DOTNET_VERSION

ENV DOTNET_VERSION=$DOTNET_VERSION

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
