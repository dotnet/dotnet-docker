ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.2.21468.3/dotnet-runtime-6.0.0-rc.2.21468.3-linux-arm64.tar.gz \
    && dotnet_sha512='cccd1e6dd00e365537b306c992ae8235a83548e9cbc0d5a7896d585e135ead4d60008432012700b2eb6fcf88cedcb692672aac080827e9acba9ffe58f3f9a57b' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.2-focal-arm64v8

ENV DOTNET_VERSION=6.0.0-rc.2.21468.3

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
