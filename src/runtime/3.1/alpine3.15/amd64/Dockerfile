ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.25-alpine3.15

# .NET Core globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# Install .NET Core Runtime
RUN dotnet_version=3.1.25 \
    && wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
    && dotnet_sha512='ae4c397f2c0ddd9da99650f2cf22d3cb2cf13aa7feb25d79992b30f44ab49989d4d96d2671caeb962eaace58f2a1dfec2e5e5248b2ed35f1d1eb44343e12ba4c' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
