ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:5.0.16-cbl-mariner1.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=5.0.16

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='95c3445413515ff6dd8d03000994506b6a960a4109983cf8f5d1eab9839beb93ecdd315680fb08add4bae7b3c25b6f44542166d6bb487489f107dcd45244c92b' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='1564bd0ccb6f2d4fdd89a8755694237632c796ac392fd4d04ffe48741c26bafa32d4bd9d8fec9823c9494e414c1c670b507adc705bf35958afdfe9b93a66aa4d' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='ca0be9b03fbb289ca69e3b25df075e074c39ced9e34bff33a99a45b070b0c7057e10f661b32115f3d184dda4b2b157e8276b0b6c3f9c2c5b4ee9697788b421b2' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
