ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
ARG DOTNET_VERSION=5.0.0-preview.5.20278.1

# Installer image
FROM buildpack-deps:focal-curl as installer
ARG DOTNET_VERSION

# Retrieve .NET Core
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-x64.tar.gz \
    && dotnet_sha512='1bed1c8ee4185c0ced69e415dc25a5040eed68d0d6a7c35c6a83fbe1eb3f71e99e8c9b631a34825141163b9fe105b281f6c7d5d39bbcf40f20b9d81fdb7384d5' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET Core runtime image
FROM $REPO:5.0-focal
ARG DOTNET_VERSION

ENV DOTNET_VERSION $DOTNET_VERSION

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
