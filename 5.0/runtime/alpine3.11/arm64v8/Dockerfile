ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:5.0-alpine3.11-arm64v8

# Install .NET Core
ENV DOTNET_VERSION 5.0.0-preview.5.20268.11

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='3817d1c697e938b19d854f07c18155e837feb237695bbd74d64e3e4b2a2c12adb05aca55d2e2a2f5835d3cfad2059946f327343a76d865a7a1c301aba7eb82c3' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
