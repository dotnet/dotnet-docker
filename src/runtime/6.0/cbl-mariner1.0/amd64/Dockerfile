ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.0-rc.2-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=6.0.0-rc.2.21480.5

RUN curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='ed2bac0f7b7aa0e5da2efd56bbf3475035c3e88544899b6b0988114dbe00acfa59df40a82da4a16d7bbb327e1a267c8210345396e9e1dd5dc3213c5769916a74' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='15e4c3817cb3476eb64029310563890972c8c17acf8b41784b9a22673bfe067972721b0166f77ebb2c8577fe4f4f49128a4dba0ea1e4d24e5f5c626c123d4e0a' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='da7afe321a67d29731db35cbf25564d0e8e4360e4e320989aa2c8ded2ae0762f507b9bfd33ff5c34d984350e07de63b8ba18d02eb4dd1487c8fac5b45b1c30cf' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
