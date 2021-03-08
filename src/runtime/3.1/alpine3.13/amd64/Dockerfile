ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1-alpine3.13

# Install .NET Core
RUN dotnet_version=3.1.12 \
    && wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
    && dotnet_sha512='f38aced794bba9a896df969667b2b01528f7857b2dd9b9018c4c2c8e1e646c6ed44e66b9b28f76bded1472fb0dd932a575c6887c5a0da9848bb3d2694751a413' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
