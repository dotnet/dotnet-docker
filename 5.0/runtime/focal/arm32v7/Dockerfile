ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer

# Retrieve .NET Core
RUN dotnet_version=5.0.0-preview.3.20206.14 \
    && curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm.tar.gz \
    && dotnet_sha512='ea49225bba22f32b3b71d02f146d10ad1388dea2930f08e994dca6ccb66dd72ff7ac4397e917be927194ec333f9c3aa059eb68c7b4757b7a0382c7c93fbf4f94' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET Core runtime image
FROM $REPO:5.0-focal-arm32v7

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
