ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:5.0-alpine3.12-amd64

# Install .NET
ENV DOTNET_VERSION 5.0.0-rtm.20514.6

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-5.0.0-linux-musl-x64.tar.gz \
    && dotnet_sha512='65aa2a661d5a9071e6a3608fdb0dfdf3afbb544c34cd5c6e3a75a65358724a172ae20a757fe527d87115b07a2ce0f9923f90e381f28b1369df2ea68ba134097f' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
