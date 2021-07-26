ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-preview.7.21373.17/dotnet-runtime-6.0.0-preview.7.21373.17-linux-arm.tar.gz \
    && dotnet_sha512='404f0b095f3fd8d737472cf9c748d0c88e49fcc002b92f18c84a39880404d9ce666c50275c92780d9f627af5b28323d3a426a6932dfce347e60e3cf2712045bf' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-preview.7-bullseye-slim-arm32v7

ENV DOTNET_VERSION=6.0.0-preview.7.21373.17

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
