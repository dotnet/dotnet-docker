ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.31-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.31

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='6c305347ee9e497093d94f16513d21a7888603bf27e8c01814ebe0afa9b4ab1a33dd4d048acfafabae412622bccdc3d666468f3294886a6207bdf0f20416f67d' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='14c4ed895f316e5d80ce765a6e9db5be250b8921c50e4a10984aacbd8aec52fb8f3196e84568c72963a96e191b51297b7e24a629caffefe858ce52888ed6b5f9' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='f8fe78865b5f7c020bd3bc8e5446751a82bb2f72924f137d8eacc59751f0962d756f3b176c8ad3dee2d8e299daaf0992b7dbbc7ae06f6fa8b1b12ef3856e55da' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
