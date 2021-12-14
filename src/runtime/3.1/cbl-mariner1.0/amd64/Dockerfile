ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.22-cbl-mariner1.0

# Install .NET Core
RUN dotnet_version=3.1.22 \
    && curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-host-$dotnet_version-x64.rpm \
    && dotnet_sha512='752fae0cb7f8f0c117c544de86560873c5250995dd897153cb9c01b9bfbf64bf84ec0fb0833dcabea5cdd6ffa295e7d1eb7ff3dcaf8e6148e3baddfff5c575ee' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-hostfxr-$dotnet_version-x64.rpm \
    && dotnet_sha512='c1513a8f9d66d0e61503ccc03536472544477b0ba09d41613709e89b1b90305bce2e5e86ff63dee0fa06aaa4d70fb1e449f3b5c20d87af2b7cb6f76d1a108529' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-x64.rpm \
    && dotnet_sha512='e3ceb3773dbbe4ec53970bbfbd85ea234952b5df6f55d24ec94249e5afab787ecfe1ee38f0f95d6235e45ab2edfd451faee9e552aaf6aaa01411f77542182cc9' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
