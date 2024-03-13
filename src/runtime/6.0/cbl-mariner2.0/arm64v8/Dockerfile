ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.28-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.28

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='74554b34f6b0d56c8204818206fd213129d7fd19c0231e2ec4a1502cd4874fe72d444e2feb1b6901a15a01cc2fd983e12f0d55d78933b0abc0fa9da1a7f9732a' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='73f7b291f4d3089e25935f014fa50a1a35971da59fd259c4d92f3572fcb52022e290d254cc1ded8dab6cb655650b2e44132389ea38014dc4b93720a7b0ad9551' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='6dc2b481e5cafe67b8919e83ba2d494ecaeb56ed698051e844291a324e4382f22d0a613e2d634d6200f23d5ecd9a64ed78096431318fb46ce60495a59e1fea94' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
