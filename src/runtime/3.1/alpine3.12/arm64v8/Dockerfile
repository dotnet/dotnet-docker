ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1-alpine3.12-arm64v8

# Install .NET Core
RUN dotnet_version=3.1.15 \
    && wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm64.tar.gz \
    && dotnet_sha512='d0d1693dee99a98227feb8d1fa56324a80efa4e54cb1aaed5c2cdce1454a328ce4625605fe5265972bb22e64ef55b66493305b1713db76fa1fd6690f6a862362' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
