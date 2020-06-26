ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
ARG DOTNET_VERSION=5.0.0-preview.7.20318.2

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer
ARG DOTNET_VERSION

# Retrieve .NET Core
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-arm64.tar.gz \
    && dotnet_sha512='cbd1edb4a54795fd7d81bf54033173416f2184af62ce2e8ba25b997cb4941ea4ead9fb2ab8f76e910de7ae0705599aa9d7972a0fc0bc61207339c8b2016198e4' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET Core runtime image
FROM $REPO:5.0-focal-arm64v8
ARG DOTNET_VERSION

ENV DOTNET_VERSION $DOTNET_VERSION

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
