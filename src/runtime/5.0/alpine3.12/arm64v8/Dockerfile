ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:5.0-alpine3.12-arm64v8

# Install .NET
ENV DOTNET_VERSION 5.0.0-rtm.20517.6

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-5.0.0-linux-musl-arm64.tar.gz \
    && dotnet_sha512='45bfd113187cf49d6858494d9ea1a2032229db39ac190aa7ed904dc7c299f930cc5b6c156316d706812f820e12aeab46d499cf58027a06e6c45327e3103cac9b' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
