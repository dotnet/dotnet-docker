ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.1.25080.5 \
    && curl -fSL --output dotnet.tar.gz https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm.tar.gz \
    && dotnet_sha512='8e91e965eace3ac1b472f8d9d2cba51537606cb0ae9b2245dde7f2f9d95f8660800cb609a7152b3efeb1e01663070c80454e6180890ccb81d6533fcc891d124e' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:10.0.0-preview.1-noble-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.1.25080.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
