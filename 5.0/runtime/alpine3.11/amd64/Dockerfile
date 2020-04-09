ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:5.0-alpine3.11

# Install .NET Core
RUN dotnet_version=5.0.0-preview.3.20206.14 \
    && wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
    && dotnet_sha512='bae14454ff067c93fd47a6c3207acd9e2ecf9023a4ffa783a3fb49296d510a3b3aaaf57c79fdeecee76e6f327a296030a512fe199a79f5b23ce48457488f70b0' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
