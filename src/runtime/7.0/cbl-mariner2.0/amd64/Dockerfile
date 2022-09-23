ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-rc.2-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-rc.2.22464.1

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='9cf54f491087b39f059331743dce231405019ffdcd754144e8cd63ac36325b2687c8f34678c7fc03a652c3a99c3a5b06c61ffced79844b8c9065e191cddf57ba' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='81f787229d9428999d601c96878fa49a8b99d96ff8fb06e59b2631b29d52c89c8ef24cc281ae6713c6d4b49f07123859596a2476ae91600fb9142a227c99223f' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='2b3a419ff9f3e8fcaa06adbdfbc293158c3f2253f52eb78797ad829308eb2211c653c0a27adf81a7a9e9624c9330c8b6d7894bc3b3e454e07041b4b6d1196ffe' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
