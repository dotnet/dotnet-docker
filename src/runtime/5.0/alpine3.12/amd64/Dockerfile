ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:5.0-alpine3.12

# Install .NET
ENV DOTNET_VERSION 5.0.0-rc.2.20468.4

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='cbf7bf6becd1049c437ac353925e68d2f534a613d60cfa65c7dfaa579cb8b48d4486c669f13dc26258800c886ecbbc25fa007c54bc87f76ff63ca075b4794000' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
