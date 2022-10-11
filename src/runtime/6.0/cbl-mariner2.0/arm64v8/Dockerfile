ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.10-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.10

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='0fd90038ca0702510f056d51863429adb7d4aa9756cfb778e00346b3d35f57dc95c33440e869f539ca00aefe3e99010b44edb91a04a8859000f310394392b6d3' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='040b0404778800cd37dad4b8fcad0efecdbdfcd77c24d16aeab3396e0e999cba8d7cc6ca2b25b5be659d48bca0154bb4566c1eed9eff5d852bf1ccd2265139bc' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='0eeac35e442d98949fb67ec39f0c4976a03143c1c1ef9f0f2cc1865f19957d9b6249a59610b78e292fd35140ee2d0d1f78483fcbbae92b66357de5483dd9edbe' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
