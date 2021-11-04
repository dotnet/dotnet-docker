ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.21-alpine3.14-arm64v8

# .NET Core globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# Install .NET Core
RUN dotnet_version=3.1.21 \
    && wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm64.tar.gz \
    && dotnet_sha512='b12a07f7c68110485157af732b19b0fc1d90916dbe5193391e92936f2f14f86884bcabeb74783d30ff74a01b1a74788b10dc093cdcaac106fd1f7da625f684c9' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
