ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:5.0.17-cbl-mariner1.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=5.0.17

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='5edc730160255ba0054e533d67a0273dbcb0e80ae95a524b48ba52c3082274a2c051c2792669ad4f2b67a71aa6e6ba02ee5e11a2eefcc849dee026a64ad04996' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='ab348ff65f06d86e749ed4dce2ca00578b6f027afb18ff89957d5fffcbd10260d88c89b258db38abcb573ef6f972df75b214106d6757687ba6e4d6bad20ef9a3' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='8382275602230bb82114e8eb7af0ddf9d0c5151756946dae178ddd29ec98ea225afbaab25f8008ddce83750dd2a304805794b6cbff8e4aa7be1dbcdd5589edbe' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
