ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.23-alpine3.14-arm64v8

# .NET Core globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# Install .NET Core Runtime
RUN dotnet_version=3.1.23 \
    && wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm64.tar.gz \
    && dotnet_sha512='af37e25e32c5f6047eb5c69b14e7a61c40c6f374175dc73ab193af4a9423270c3a680824b9f5038545e4b2da5c36fcaf23f6c9bbf09123410f96f78c4694d58b' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
