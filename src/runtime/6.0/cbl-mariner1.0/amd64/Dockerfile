ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.0-rc.1-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=6.0.0-rc.1.21417.1

RUN curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='494a0fecb4563326fab34730e7cd4e8a4129a57a79bcbba3b4e50e9b4d554df4c412e3cf9127f7e0e8abadad03603b21f47966701c2bb8be37ad27f93cf5293f' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='d0fc3d905dddd63a1fbe40274bc8c6cbf58c200cfe00aabeb6522164adffa10f582e833d30a479f20478f5ad6aa0394e7f8afd0d0dbc1f835fd60f054d324e0f' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='56a6e64055f911ce3cfeadb880f1f1327d0fd5bf0e75480efcab111eef2e8e5cfedded022c7cd78a1a53a18840223097a9b4465725a9daba16272b5413df5e5c' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
