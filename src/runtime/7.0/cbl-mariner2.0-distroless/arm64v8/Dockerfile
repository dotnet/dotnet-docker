ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM cblmariner2preview.azurecr.io/base/core:2.0 AS installer

RUN tdnf install -y \
        ca-certificates-microsoft \
    && tdnf clean all

# Retrieve .NET Runtime
RUN dotnet_version=7.0.0-preview.3.22151.6 \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm64.tar.gz \
    && dotnet_sha512='e3deae93c5d373406c691dbdece9835c361b2ad7d92f37a36ec13f55ac4263ad34d089e1fe2bfd01735857ffdcc75713f2b7fe7a5bad6cc9721960fa0d603468' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# .NET runtime image
FROM $REPO:7.0.0-preview.3-cbl-mariner2.0-distroless-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-preview.3.22151.6

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]
