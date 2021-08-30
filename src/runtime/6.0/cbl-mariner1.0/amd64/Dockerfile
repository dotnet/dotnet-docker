ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.0-rc.1-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=6.0.0-rc.1.21426.17

RUN curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='ed41ec0352db86f9e928c148614a38aeaafd0edb674a4081b44eab2432ebfab1f8968aaaf69a4c461b299ea1290c0d35e4213fb14318f6ebda51b681bd57585f' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='30e91ed78fb1d26506226496a0bd1382d01248b263e981e1108a011084d531ab149a7843a6d70a97f3b5f6023c2438e801fbea4f07feb9273670a9e30a22ce1a' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='2b36a53e1e63fe941cf45d0e09651d2119e342969b144e3deb4471181b2fd5b7f44e7f18d33b42954a82c69038d2dd09bfbf6047490f199c3e11771475f4cb6d' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
