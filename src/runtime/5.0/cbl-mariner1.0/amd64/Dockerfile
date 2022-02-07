ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:5.0.14-cbl-mariner1.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=5.0.14

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='6b766769bb92fe4021a5dc77ad54639bc2043a3aed26eb66dadf52595f53d53bc8da40356504a98dfb41ccf79b24866f374c79a17c84369c0c53df392b2c7d78' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='4bc117b479553a7e40f142c3da89d2416e0fae04ce468a680dc8ccdcaeb9956bcdf448e76254a71001816f133f471bda87f339b999b178da4ce6833e9e29d288' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='f301aac02c71a99ee672db36d0ebc8d786067a0715483b83adb01a243d3e216098050030a2173110ea76f6b0e97a0d1cdff216880a0b5afc1abc237fde27a7e9' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
