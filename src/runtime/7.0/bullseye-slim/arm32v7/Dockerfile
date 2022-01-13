ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl as installer

# Retrieve .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/7.0.0-alpha.1.22061.11/dotnet-runtime-7.0.0-alpha.1.22061.11-linux-arm.tar.gz \
    && dotnet_sha512='36a58380961376f68837468fb1ff74ffb12dc55897b986b39cac9b9afd52c6c8fc61326521afe712a0c1cb6176e5116f1034caf557f7f5863307073dc6114733' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-alpha.1-bullseye-slim-arm32v7

ENV DOTNET_VERSION=7.0.0-alpha.1.22061.11

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
