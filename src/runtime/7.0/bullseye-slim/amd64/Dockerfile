ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/7.0.0-alpha.1.22056.10/dotnet-runtime-7.0.0-alpha.1.22056.10-linux-x64.tar.gz \
    && dotnet_sha512='67a1451c805d07afb26e902be2f88cb9fd831c38ff865795b78534f12af952d252bdd34aaaa3fe98550067eebe91dc776abdf154dce196e8376c43beaafc1a78' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-alpha.1-bullseye-slim-amd64

ENV DOTNET_VERSION=7.0.0-alpha.1.22056.10

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
