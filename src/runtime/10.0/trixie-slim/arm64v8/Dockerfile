ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:trixie-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.2.25156.1 \
    && curl -fSL --output dotnet.tar.gz https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm64.tar.gz \
    && dotnet_sha512='1ad8f2f2caa8d81ae0c68a0fd831667a0dd0976c81cf924a035f718290d32196495d8261cc836c309d7833e386ef3938034ade97446e51ec92d23e309801bc1b' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:10.0.0-preview.2-trixie-slim-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.2.25156.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
