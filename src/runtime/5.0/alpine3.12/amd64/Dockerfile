ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:5.0-alpine3.12

# Install .NET
ENV DOTNET_VERSION 5.0.0-rc.1.20430.6

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='98af33a05328bae763c07bb2506715f6b7ad606f9073ac282b8c0c42476ca0f6e2f4e72f6bc5a44ea0d763e09506469452aad04e1c35a1263a4c4824cb42b9e7' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
