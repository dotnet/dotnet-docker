ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:3.1-alpine3.10

# Install .NET Core
RUN dotnet_version=3.1.1-servicing.19575.1 \
    && wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
    && dotnet_sha512='cdb932177f6440da51e0fe9fabd385fc8a96fe67acb51d0771d326aace0a2769f9e55274e80424dcfcfbeb9ba300a37dd9cd24db72dd2147990b7cfc5d0121fe' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
