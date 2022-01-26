ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.22-alpine3.14-arm64v8

# .NET Core globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# Install .NET Core Runtime
RUN dotnet_version=3.1.22 \
    && wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm64.tar.gz \
    && dotnet_sha512='3145597fe5fb4d8d08839e744a2c517591a03c6d287c02390a65eb99d4559a9ee593568a09044fe1d583f44f4a398cf0c309b7f6c5bea0d6ed5d63c57bdae650' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
