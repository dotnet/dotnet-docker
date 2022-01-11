ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer

# Retrieve .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.1/dotnet-runtime-6.0.1-linux-arm.tar.gz \
    && dotnet_sha512='a6bea3289279ddfaeda4c46bc2cae82c662e2b2cfa13abefa18baa8db747bd1be9fabdd8c0103310f89d4c9e356235551af5354742117d2cd58abf5727883609' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.1-focal-arm32v7

ENV DOTNET_VERSION=6.0.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
