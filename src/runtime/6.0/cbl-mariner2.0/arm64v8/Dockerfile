ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.2-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.2

# Install .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-arm64.tar.gz \
    && dotnet_sha512='5cca54e18332b3297960f21a1b523382e2e7ad6fff477548ac0964451a7db5fadb2acda7cc39bceab184250bd9f81f84162aff4a2ec1d2c4ad278985ed157f4d' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
