ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.27-alpine3.15

# .NET Core globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# Install .NET Core Runtime
RUN dotnet_version=3.1.27 \
    && wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
    && dotnet_sha512='20fed726e85ff1aed1f53afb4a193fdac83275636f8a9370b0de8498689e0f82919c45df5aa128e68f5323d2cedc8b4ecafff192ac8765f2e2cdacd6ecb8a920' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
