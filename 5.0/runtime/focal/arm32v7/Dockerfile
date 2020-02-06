ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer

# Retrieve .NET Core
RUN dotnet_version=5.0.0-alpha.1.20103.10 \
    && curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm.tar.gz \
    && dotnet_sha512='fbc26aba4ad0bbb74ea6209058e98286099ea4560c7b4f4ef41ae36c2a6137754c7724a235ba75c265a77c89b6bc702648fe173de6a563a3b128a7b466bb6e84' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET Core runtime image
FROM $REPO:5.0-focal-arm32v7

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
