ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21414.4/dotnet-runtime-6.0.0-rc.1.21414.4-linux-arm.tar.gz \
    && dotnet_sha512='0de06c5a9028a11cceba6d643038807823aa7d0a0da7aa2bd26f33d2c9c7172a06a7c3c1a4d4fe413d99838e6f6bdd20db16eaffb7e30d8885d99d23a8306c68' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-focal-arm32v7

ENV DOTNET_VERSION=6.0.0-rc.1.21414.4

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
