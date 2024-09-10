ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:bookworm-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=9.0.0-rc.1.24431.7 \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm64.tar.gz \
    && dotnet_sha512='8542bb9381e4eca6f0ebceddec68525cc59e34f7244613cf33cb2151f570c3345cb6d081c492b01070e762d3440f02d4558234532d58ff3dc919057e06b7bdac' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:9.0.0-rc.1-bookworm-slim-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=9.0.0-rc.1.24431.7

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
