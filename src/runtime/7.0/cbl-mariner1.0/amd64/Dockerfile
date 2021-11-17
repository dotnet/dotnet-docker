ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-alpha.1-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=7.0.0-alpha.1.21562.1

RUN curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='6081502ea7568fe006d782c1e17cf0e5cfa2806b43b8d94f6ff0af8d6b76e345493acc649938cbcd0fb4726e05f2d9fc468bb2bec329c0779f75758287a70852' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='31132c7d28d3fb9fe2e101f0d5355ec68e2ebd653cc6e1ae804cdf4ba93d6feda8c98f71108bdaac20cfcb36fb6bb033d5f7a3edc31c2df3a7ce4ff0b6623296' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='463570689e61c00d2c2347c718412337c3a9a8d616892bce624a98a7d978809c3bc9f0c030b43ce1a338585817a64fa152c21b6fbf08bb31f31997ed385c86a1' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
