ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:bookworm-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=9.0.0-rc.2.24473.5 \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm.tar.gz \
    && dotnet_sha512='c3ea1494aed56c557406786e16dae25a2d1b09e086fa470bee7850203f3c995ff0878ba36707a11719db1e517c6fcba53b103a6987b4fda9158df536cbfd27d0' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:9.0.0-rc.2-bookworm-slim-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=9.0.0-rc.2.24473.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
