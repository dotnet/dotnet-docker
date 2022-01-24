ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-alpha.1-cbl-mariner1.0-amd64

ENV \
    # .NET Runtime version
    DOTNET_VERSION=7.0.0-alpha.1.22066.4

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='602461dad16d20fb07b33a1e535b049b8f8670f08a032e3a13e13cd6f2311e95fdfb4c5b88369d596a3e9de0169a8f21d8de4d5f0858814320d6fe4f09a1b9c1' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='364e7f016d5b2a6e6110745d772739bf152ffa597248a8bb700c33e587128d15997dbf5f6c88c3321eb5ded1438662b0426e4a4a1b632eab5014351e2af96538' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='e103db5b3fa18f50b83a62c1019ab1351d229f2e07ca56451eb6b414826cb63868dcd66075eb19857965de351b88233dcf43fcd90a1904ea1d62d845fa9fc6f2' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
