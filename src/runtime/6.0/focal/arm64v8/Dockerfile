ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=6.0.13 \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm64.tar.gz \
    && dotnet_sha512='f70c8e51c06f9563f33a9a1486fba1487c7ca8f2f77a2de02d382f82bb041b6c432b9cd3a399a4a4e0188b8aa96dc4cc78e9147fa0d0c4fca7fffaec55c38903' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.13-focal-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.13

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
