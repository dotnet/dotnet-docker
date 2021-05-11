ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1-alpine3.12

# Install .NET Core
RUN dotnet_version=3.1.15 \
    && wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
    && dotnet_sha512='c6686acbd398ca9ff5c47cde4a59f38794b207f2f3bd00d8f2b7e1dea04160f495cdb39f79e663d413d837a131c45bc9ee03653b59734cdacaa2c2fc0a57a002' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
