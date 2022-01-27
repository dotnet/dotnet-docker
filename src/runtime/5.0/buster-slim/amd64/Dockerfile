ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:buster-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=5.0.13 \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='32f574369975606da5e67ca7f8e709e73b67a98fa1a175f49c979f582be658132375663e6ad944a3a89d1186322a7a04cead043cf1d4526fdb7ff195ead6f317' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:5.0.13-buster-slim-amd64

# .NET Runtime version
ENV DOTNET_VERSION=5.0.13

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
