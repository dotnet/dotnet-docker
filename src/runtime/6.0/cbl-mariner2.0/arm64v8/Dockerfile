ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.19-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.19

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='a74a3e1d21490e772016887c58d3fde71b74399b9d4e5afc8c737198d47eb7db6011bcb559e551f972b5d96ae56f3b1eaf5652a35f94e282e081e70460a8dba2' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='8cc5410666675bcbe42a4e02aeffd5fa1d697bc47fd922343f8a84e77f503b88d1e32fad1bb22d680804c3c66a3a14af597fafdbb75ef11d3eec4be95fdccb0d' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='1ec5fd1dad5d8cbd8d3b3b3140cfe33d589e1f5208ced48e73ba8adb3bdce7371716b2726e092122abe6ce5f1fa370a7998c4d7b0a7ce2f1de07781593e9ce40' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
