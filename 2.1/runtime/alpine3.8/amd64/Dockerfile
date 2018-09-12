FROM microsoft/dotnet-nightly:2.1-runtime-deps-alpine3.8

# Install .NET Core
ENV DOTNET_VERSION 2.1.5-servicing-26911-03

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='0e30ec329557393f4b885c29c02ef2baf3de8209aa609fd3486d82fc95cc7bd8257f0b2cc729c7a62a263178204c186c1fe15f0949ea9eabf2218173b307e06c' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
