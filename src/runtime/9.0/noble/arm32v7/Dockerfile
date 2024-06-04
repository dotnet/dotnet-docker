ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:noble-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=9.0.0-preview.5.24280.5 \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm.tar.gz \
    && dotnet_sha512='890b7cba6ccaa250d4f4acbe6e14e0af390e1d9325c1bcee712cccc42da2151112b67734c5efda6d96793b13d57482092d60f9b77666a4e5a77098a938dfe752' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:9.0.0-preview.5-noble-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=9.0.0-preview.5.24280.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
