ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-alpha.1-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=7.0.0-alpha.1.21567.1

RUN curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='bc1537240c31d4353dc1ff92e72e88d1f5fb0266245add09fa2f7294887737a4f64fca3a8944642520ed152327b99d9e885a5e0b189b5500475a51cada781d3d' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='f6d1958247b9b68acdb84c0f6c18bac17d820bed2b8a1c581d38ed80026dbce98eef693fc87bc70566a4829a387cb5f6d0c913d53c8e5934c6029fce4648028b' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='99a279a150c926127927c8e9f7be084820b8f523a3ba52810c94d1adcbe23eb12acdb8be9c21d0c2c98493a991a87415404e3d2b82261c51cf4991690eac2d62' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
