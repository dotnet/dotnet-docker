ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:3.1-alpine3.10-arm64v8

# Install .NET Core
RUN dotnet_version=3.1.1-servicing.19575.1 \
    && wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm64.tar.gz \
    && dotnet_sha512='6e7a674cd70f37fea0576c760a85d6005e18f0970cc40f2920e9b86182f1ad27fed97d486535e623d1a2b660c8f08a11152478d3a22e4df6cff90b27ef97d4b7' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
