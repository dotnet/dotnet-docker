ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:bookworm-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.6.25274.104 \
    && curl --fail --show-error --location --output dotnet.tar.gz https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm.tar.gz \
    && dotnet_sha512='9082bba17d6907462e05f4f458fdf1fa63eed6b5775c03e4b49d5cb3a2e4e1526cb7140043a9d17f2c4afec803d74885b203f2b22aeadd118dfbc38c2b19913b' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum --check - \
    && mkdir -p /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet.tar.gz --directory /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:10.0.0-preview.6-trixie-slim-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.6.25274.104

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
