ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21415.6/dotnet-runtime-6.0.0-rc.1.21415.6-linux-arm.tar.gz \
    && dotnet_sha512='72a17249c82afefba91d8db723dc9a59af5ec48d09b0a40c875a826294cf29083fa4cb4768aeec5d133fae94986310e998556efa594a8f91c05bcdb1a0e1c9b9' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-focal-arm32v7

ENV DOTNET_VERSION=6.0.0-rc.1.21415.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
