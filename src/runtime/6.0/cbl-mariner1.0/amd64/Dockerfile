ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.0-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=6.0.0-rtm.21514.7

RUN curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='5f35d2feb93128c809cbfcca7f540dc976bfaa759fba3d594ff03bea966b1ed104b37210838188611c7d6bc8d41f79ede98ad5c787a09b32f2ec25339c0e5092' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='ce3de34faf4490544a4aef47d7ce336e5bca682f4ec17792e7ef86483dd99af214b1af81b35cc590830d531b7c9bcc4aecea79ab17cb40af842e8cf9150b35ea' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='e76c03fc4c2ff1ad281db10baaf4b4c1e2aa5d501ab76c15a17e3b6d706bbadc866413d4d28ff32c8b1737bdfad064a4b8eab4666172e7d9b9ea7eb6c8080356' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
