ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:bullseye-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=7.0.0-preview.3.22151.6 \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='e93d54ea828aa94d00c14077876ae34842d1b2ea4c44722f6b4f3e04ac49acbb28d6cfcd33d39d26376196a557990f22140a213a1260d4cf7dea3d6e993fe3cb' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-preview.3-bullseye-slim-amd64

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-preview.3.22151.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
