ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21426.17/dotnet-runtime-6.0.0-rc.1.21426.17-linux-x64.tar.gz \
    && dotnet_sha512='2884cf5c39fda977a0b359c47063fd145faceaec385ccb2e31e81945e0326a2ff5539fb595f2ed42f41d4cad83d5ba9bd4cdff43c54e8baf8a8fece316ab1d58' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-focal-amd64

ENV DOTNET_VERSION=6.0.0-rc.1.21426.17

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
