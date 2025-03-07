ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:trixie-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.2.25156.1 \
    && curl -fSL --output dotnet.tar.gz https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='d6f0b0e6c88b7c87b2aedffebf4f57257fd8e6e10d6b386d3eb9fe3a27afd26b28971f2f34e0e5776552dd4e57baa67678a9faf2481ec38e13c59af0eeb75851' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:10.0.0-preview.2-trixie-slim-amd64

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.2.25156.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
