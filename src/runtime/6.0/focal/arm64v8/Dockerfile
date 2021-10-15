ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rtm.21514.7/dotnet-runtime-6.0.0-rtm.21514.7-linux-arm64.tar.gz \
    && dotnet_sha512='1a70575216521a4cf01651a828c8a468e0fe4e898071aeb2b4fd7e39a35fe701716bee9e7e44623720bae307f9b12e6343d3f988190a8b219b2a5c1ca8e7b292' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-focal-arm64v8

ENV DOTNET_VERSION=6.0.0-rtm.21514.7

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
