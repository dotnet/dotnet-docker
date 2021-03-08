ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1-alpine3.12-arm64v8

# Install .NET Core
RUN dotnet_version=3.1.13 \
    && wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm64.tar.gz \
    && dotnet_sha512='9e6c6339c467bd96801df1678f1d4e0f896795a429d7848675176df7b781c44b0c984ba6bc6b1f08977290f8317d5ee96670c316b3d6b77e2d316045e5c2317a' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
