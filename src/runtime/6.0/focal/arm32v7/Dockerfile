ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
ARG DOTNET_VERSION=6.0.0-preview.1.21074.3

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer
ARG DOTNET_VERSION

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-arm.tar.gz \
    && dotnet_sha512='8eaf48be42e53ed720f381bd36513640b2c3e09160aa9ea7a2211d8ba1ad22896b466922832de61205b00e55ac7af3372bd3d0d51c85943ef131b639d22b5302' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0-focal-arm32v7
ARG DOTNET_VERSION

ENV DOTNET_VERSION=$DOTNET_VERSION

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
