ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1-alpine3.13

# Install .NET Core
RUN dotnet_version=3.1.13 \
    && wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
    && dotnet_sha512='40f23e81ca8fa8bcb657e480a475650b2e3c59daad702e2cce0ee8daba18e9703f03bb02a28bd9ae548410b0f503ebdaa6de1079b417798f965217fc0ee94cd0' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
