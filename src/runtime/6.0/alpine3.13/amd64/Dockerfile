ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.0-preview.6-alpine3.13-amd64

# Install .NET
ENV DOTNET_VERSION=6.0.0-preview.6.21352.12

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='ccd1da6558a1d54e5ad4b651684990d56b7516c5968371f04a9d8f59c847073270ff916b185a1cbd93ddb362ff44e358db1be0335af12bbae0aa22db845057d4' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
