ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.36-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.36

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://builds-int.dotnet.microsoft.com/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='00d9f978054aef3ec4b2bd81eedee8184e4fd44eee364d82b81eab7079958a9d0fe6cfdf5f29c05f33a20e2cfc9761306a8bcc378dbc804cf1f38eb9ea29871e' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://builds-int.dotnet.microsoft.com/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='79faa94cef34307a1d947300755e002056f42094003b3fc3447efea6731ddece3d6d18ab62c0292498418a627ba395c6c2053a8b92fbdac356cd0afb141e7e7d' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://builds-int.dotnet.microsoft.com/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='a3a544b6d315daa8e4fceb75d7414502d5b8fa5c6f7dc14c5ea05a8c32d50adf8422471eac69893eb8ea10ff908879aea277fc2b6aa5a723b3f60cf3c2e84c7e' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
