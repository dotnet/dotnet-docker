ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:bullseye-curl AS installer

# Retrieve .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/7.0.0-alpha.1.22065.6/dotnet-runtime-7.0.0-alpha.1.22065.6-linux-x64.tar.gz \
    && dotnet_sha512='d6d75f774abd85eb5e97618b1a91ee33cd20a122a094f8f405c69a44e8f26a6182193c0d1f4f31edc13f89c953a727f6137574a8cfe8340b62e725b8a36e3aac' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-alpha.1-bullseye-slim-amd64

ENV DOTNET_VERSION=7.0.0-alpha.1.22065.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
