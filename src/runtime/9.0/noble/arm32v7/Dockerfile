ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=9.0.0-rtm.24516.10 \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-9.0.0-linux-arm.tar.gz \
    && dotnet_sha512='ca5e9488397b66d20d39b7d3d0cbc28c7451a402ccce35ca5118070507502a7d746d2ed1aa45f3c6f8ebe83d954f3836d536566b1560d9482e7c00b4dc97fbf1' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:9.0.0-noble-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=9.0.0-rtm.24516.10

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
