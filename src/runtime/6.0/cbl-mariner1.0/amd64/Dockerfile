ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.20-cbl-mariner1.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.20

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='dcca1e2fee66f6c18396f8b88204facba0208ee9facaf39aba494a1f50bc664e1e0f0b2f8c53adda45d26ca09612c9c0ad66faabc2d922b9f9057332c795fd2b' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='49ba8b6674802ad29089c1d2203f06d7152ca632c2cf85a11d313be46f8f6b505df840f034b81baf43ae9810b3b4c980a3b3e6619184432c9b77bbf1b73328e3' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='bb72898d7fdfa7abd0fd8528283ad147afcadf18cbace073c67de434c21dcb551dbc568d3e809301c95b0ec68a400e1aba5bd33aa6440dec81051c50e1d60758' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
