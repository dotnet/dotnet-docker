ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:bullseye-curl as installer

# Retrieve .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/7.0.0-alpha.1.22059.2/dotnet-runtime-7.0.0-alpha.1.22059.2-linux-arm64.tar.gz \
    && dotnet_sha512='334ad8140bf7b4eb546f7fb46a7def00f2a798d6c6243844540863942cb537533d3b7e361a363db355c5a1af8441dcf9ad4063bc416b8710ba43d95007a82e81' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-alpha.1-bullseye-slim-arm64v8

ENV DOTNET_VERSION=7.0.0-alpha.1.22059.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
