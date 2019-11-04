ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:3.1-alpine3.10

# Install .NET Core
ENV DOTNET_VERSION 3.1.0-preview2.19525.6

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='b4cc00661c6b470da2aea56ac9ccb12e64bb6ef26c388b8b1caa9c14dc7ddcd2c87d55fd7f46c83d352f08edb1f709b9d2c6a68688e96af0ce23ad115e2d0519' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
