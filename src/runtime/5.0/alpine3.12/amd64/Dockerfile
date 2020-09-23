ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:5.0-alpine3.12

# Install .NET
ENV DOTNET_VERSION 5.0.0-rc.2.20472.5

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='abf135be94ad56d3ea2b018e49fe983981371c9a4c79462146b46ba6091d964072b4f56fe72a5b98278c289e04c67e7686cd707d4d2bdeae5fd790c69a6babab' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
