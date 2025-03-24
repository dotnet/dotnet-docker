ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:trixie-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.3.25171.5 \
    && curl -fSL --output dotnet.tar.gz https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='4e9bd2bdd54124c95dd62bcf96a5feea095617d73f132cfcc8332d15a9a8366c03bfe55d02e7409ea8ee3d35da1eb0e98390b9ec3fc86e928c6706c535fe67fa' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:10.0.0-preview.3-trixie-slim-amd64

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.3.25171.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
