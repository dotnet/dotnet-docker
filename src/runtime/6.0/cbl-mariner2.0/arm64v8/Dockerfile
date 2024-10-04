ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.35-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.35

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='0c4137f5de4b13ad577a0b07c1536c2a48a887be24afd3dd698bb8d0abcec04d8631526cf1c9e5b14decdfe7a3486f513c91ed3cd52a5b5ee88640c006ca098d' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='c9bea9552c25d2c2d65437ccad398761676d0fa729c0c357eb5fbed74da177a89d7e28805a380423324ce7091b3a6cfa29bca547e43b45251f45192466a7180c' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='d288052f1e3aeeab94e1045a56a9c09b8f856444b4ed590eadb72dcb666cfb28832739b3866f2c1f915284e425d0650ccf0f64ad2759c67237016607cdf512ee' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
