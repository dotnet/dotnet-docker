ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:5.0.12-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=5.0.12

RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='3dd1b95ee725a8dca084790efc02181e30453a61051a6e076ed1746d7ad721bb14311f7d31a14092ffa022c862bdeb1e5f2733613cc2e87df34cb9a1aa4adda3' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='f77616b491959636c2a452e6344b3eec90eea715694d2b1b25daa5fdf3bedd78de8515791f0841ccd886a21a1370cd7152c048b5e5934d419b5a498711b96626' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='c29b806b9bd441e7f06ffa5fb1a4d4243a2e3ee371e5c5ad08df83776e593f73a17f5195845bdda9b175d39042653ba1e9a34e0c8509889b92410a8a9dd6ac42' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
