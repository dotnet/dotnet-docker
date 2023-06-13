ARG REPO=mcr.microsoft.com/dotnet/aspnet
<<<<<<< HEAD
FROM $REPO:8.0.0-preview.5-alpine3.17-arm32v7
=======
FROM $REPO:8.0.0-preview.4-alpine3.17-arm32v7
>>>>>>> upstream/main

ENV \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
<<<<<<< HEAD
    DOTNET_SDK_VERSION=8.0.100-preview.5.23281.8 \
=======
    DOTNET_SDK_VERSION=8.0.100-preview.4.23260.5 \
>>>>>>> upstream/main
    # Disable the invariant mode (set in base image)
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-Alpine-3.17-arm32

RUN apk add --no-cache \
        curl \
        icu-data-full \
        icu-libs \
        git

# Install .NET SDK
<<<<<<< HEAD
RUN wget -O dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-musl-arm.tar.gz \
    && dotnet_sha512='c5bce83eaa0abc013c0506a07a3db6a2836809a4ae92ff75d04e9fcba0299e4b7c01e307aa293d6e8776151faea943bec9bf95f5c24c344a6d7f62fb07d9e5be' \
=======
RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-musl-arm.tar.gz \
    && dotnet_sha512='78baacf48697910adff90fe1d9227926daaeb71365dbb6170f0a5d716ad474b3cf56bad08d8c78f5ffb8538e54171855eafc0a92cb2c8b9b7d30ee25c012866f' \
>>>>>>> upstream/main
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet ./packs ./sdk ./sdk-manifests ./templates ./LICENSE.txt ./ThirdPartyNotices.txt \
    && rm dotnet.tar.gz \
    # Trigger first run experience by running arbitrary cmd
    && dotnet help
