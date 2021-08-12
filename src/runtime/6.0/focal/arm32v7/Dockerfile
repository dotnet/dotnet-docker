ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21411.2/dotnet-runtime-6.0.0-rc.1.21411.2-linux-arm.tar.gz \
    && dotnet_sha512='30cd72b868242b66a293fe12954914aae23a6aa248d64be1cd9f144eb28508b325240db9605c36316f99b3120b100ece412aa454a89ba3200588271e97349afd' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-focal-arm32v7

ENV DOTNET_VERSION=6.0.0-rc.1.21411.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
