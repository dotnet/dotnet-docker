ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-preview.7.21377.12/dotnet-runtime-6.0.0-preview.7.21377.12-linux-arm.tar.gz \
    && dotnet_sha512='b629a72182ad7bc83c99c2732c642b43452241a77422a1383c41e8120bd44d01b9b627aeb5b7f37931d1696e2a9007d5ba6534084ef538a6d060089573d8c3c9' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-preview.7-focal-arm32v7

ENV DOTNET_VERSION=6.0.0-preview.7.21377.12

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
