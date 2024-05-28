ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.30-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.30

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='80a494c9089c845084b51c77069837decc258911a6275287497166e287b8c5610d10567fc0e6c0e8300119c1c47cf71bc291533f6a0bd67eb84a06ad7b8f5ac8' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='d5b6759745c56df7bb3cecd6d4707789e89a22e139647b329bd0e7a86bcbaa8b5a4fd3d921ca47750d92893b76e251a06b7d2daf9cfca2ca497f24326ef8a83e' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='15b9ffc2fe64591de48c7a8c716f17193151202edd356463a337bac8e919d5b2264c6b49cbf8e4af111ba5364701e66644631f1314fbcc010ea48632e4536a69' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
