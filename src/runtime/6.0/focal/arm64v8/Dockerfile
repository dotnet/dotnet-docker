ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.2.21470.23/dotnet-runtime-6.0.0-rc.2.21470.23-linux-arm64.tar.gz \
    && dotnet_sha512='3263f1957cf18d0260380208c48334c38dce655f7c302b71277a2a5062e5d7b933c47b4028b985a8477bc81905aac8848d633eecc705bffb733d4cfd010ccefa' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.2-focal-arm64v8

ENV DOTNET_VERSION=6.0.0-rc.2.21470.23

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
