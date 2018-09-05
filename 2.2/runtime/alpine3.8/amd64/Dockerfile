FROM microsoft/dotnet:2.2-runtime-deps-alpine3.8

# Install .NET Core
ENV DOTNET_VERSION 2.2.0-preview2-26905-02

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='ef90cbd272f426bdbf8737a99229ccd0eaf9cc6b0a5a910c28f9a892ce11eacde33b8c74bcd35cd2e458ef811295aa9ca92a4ac28db105a3cfdb9e6d6ad3470b' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
