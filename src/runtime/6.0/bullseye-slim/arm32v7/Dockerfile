ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
ARG DOTNET_VERSION=6.0.0-preview.4.21253.7

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl as installer
ARG DOTNET_VERSION

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-arm.tar.gz \
    && dotnet_sha512='9006314540937845adb3e8746fda48037bbbfb28cd76eeb66655d1d1a92014d979256cb78b08f5f85fe8b5b8952a10b3c152be99b7dda3b2bfe1ecec5935126d' \
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
