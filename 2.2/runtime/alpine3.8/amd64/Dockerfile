ARG REPO=microsoft/dotnet
FROM $REPO:2.2-runtime-deps-alpine3.8

# Install .NET Core
ENV DOTNET_VERSION 2.2.0

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='10665979bf77ec7cf92108372c7ff48b4f0e577def75513128d088567e5a2553ff1502fe6e4e403d0db485d629a11eab93cb5fe84752a0a8d15398e1f2aff53c' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
