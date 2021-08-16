ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.0-rc.1-alpine3.13-arm64v8

# Install .NET
ENV DOTNET_VERSION=6.0.0-rc.1.21414.4

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='0b877b71adb86d4e7763e234734d7c733a2a31338bda07920202b9332a0f8e81c995b6969f3cb699f97d22364c587cf75fbe028e7bf0b7f86a4482065561d1e7' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
