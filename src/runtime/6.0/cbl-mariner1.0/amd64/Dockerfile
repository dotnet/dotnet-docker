ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.0-rc.2-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=6.0.0-rc.2.21468.3

RUN curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='e4f34996a861ee8ebab831416b6ec796c7650ab36375d9259f9f67ffb8de6e8e0364c6d8f709f79ad270bc2298c8cb44dd47b03ff37e243212b78a01f08ab33e' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='7083a0eb913ea32f0e98ec31e69ec1e7f09dfd3d61764f15b22dcf317f10e07ebdf076680689eef07d279be5a7444aeabee64f494addba24dbe38a50b7ae27cb' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='8104731b67c1f8f1ecc3fe8e4a1b6f4702af52496ec0b1be99201c0545b59d87881a0b7c10ad5d974775a4ab731b8dafc0f3a5a3dd6c8cfbd4604c77cbd6ed5a' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
