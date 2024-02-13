ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.27-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.27

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='b6d4c302c7670643ee26bc28aab3c99d7a1cc28a1f5d8a387593a23dd94e1ba367ed389691882c84848ea993e3d649d727c01ec835e3a806462e22b593a9c96f' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='689b3a327ead221e4c9a8fee9597c289009161fb6ca418a362c120e0f84d6c9654d09e3d384c6787e0783f852e2f6da93c0fe264226d1a788d5f230bdfaa834f' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='c3234c57cdf5762aa38c26056fed5dd3593dff5e995ed9f2f73234ceb4f9354e4f3ccee54a82f3bdf0ec916c2ef751ea412960907b9f6e0ade682c8cc6dd78d3' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
