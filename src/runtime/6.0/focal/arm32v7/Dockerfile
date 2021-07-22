ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-preview.7.21371.11/dotnet-runtime-6.0.0-preview.7.21371.11-linux-arm.tar.gz \
    && dotnet_sha512='a4b93e8622b6448e966183e21338e4cb490425eee9eab4cb7c0e926ab200d0f5501797433c9c6c6b8b908efe2753cb241a5e093963f99615f16f130fedf485f3' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-preview.7-focal-arm32v7

ENV DOTNET_VERSION=6.0.0-preview.7.21371.11

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
