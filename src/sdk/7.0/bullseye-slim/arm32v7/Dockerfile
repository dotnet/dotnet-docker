ARG REPO=mcr.microsoft.com/dotnet/aspnet
FROM $REPO:7.0.0-alpha.1-bullseye-slim-arm32v7

ENV \
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS= \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=7.0.100-alpha.1.21568.1 \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Unset Logging__Console__FormatterName from aspnet base image
    Logging__Console__FormatterName= \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-Debian-11-arm32

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
        git \
        wget \
    && rm -rf /var/lib/apt/lists/*

# Install .NET SDK
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-arm.tar.gz \
    && dotnet_sha512='b444c83655c2bca07843b4e8bc9a93f9d25f7bf3ae59493426e762728bb9bc237acbd6ce87fd497a6bd7966d763771ea5b3c353ff5e19cb27c005ebb56996635' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz ./packs ./sdk ./sdk-manifests ./templates ./LICENSE.txt ./ThirdPartyNotices.txt \
    && rm dotnet.tar.gz \
    # Trigger first run experience by running arbitrary cmd
    && dotnet help
