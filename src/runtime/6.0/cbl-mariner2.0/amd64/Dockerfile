ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.26-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.26

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='b6b688f4bc0c7894abf1a69e26b576d01fc0e9e5873af75bbc77196e21c2e12b1fdc53638fd765c9e76b37f1984826bc13330ccde2aa5388e2a71feb62b8d9b5' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='ef3b6dad553bab4aaccd13d73e780951c6bc75779951e0309fdce2fe2721aa39065c1861cce1147fc50ebbdf289e65ba3c94ab5c3742753b90337aae01f7a3fa' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='7771e434ede9ff4362a51cf3092b2ff62769b2ba3ebd4baf222e2cac429bc5bcbf68181eb46dd901630469c6e37a37e45ea577dc45ce7ab69188275b0962842b' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
