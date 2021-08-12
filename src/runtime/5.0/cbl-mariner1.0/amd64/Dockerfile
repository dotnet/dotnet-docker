ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:5.0.9-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=5.0.9

RUN curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='4c472c324c9a6a25603f204c51206e9d1f1caa9d42c55b07a5456975fa4b1e6e1befa3cf406546ef3fdd8a6bfc3a108cdd9e408eac41608d86def0371d48b1b6' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='504d8b862bfcdbeff9b071724c99b7850a0040b1fc73520c30299edd7d362f44aecdd1d446ca2069b82925878ea696410a751523b19810fed007b2d5c9be2069' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='826fb0239ff72caa25dcfb53a853ffd8beae0cf614825d9bd94a1e31d793435f46de164f1f99aeeaa5ed05fc13e5c8d34010f9e43726ff2221bacf4e834da3ee' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
