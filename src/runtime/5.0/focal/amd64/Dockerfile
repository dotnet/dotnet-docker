ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
ARG DOTNET_VERSION=5.0.0-rc.1.20427.12

# Installer image
FROM amd64/buildpack-deps:focal-curl as installer
ARG DOTNET_VERSION

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-x64.tar.gz \
    && dotnet_sha512='836818388118460198ea8a3aac387d6f7187cd58f4918a4693e9619fac7f7b89c26edc2727f5222759cd21fe4c75b9c6ae58e6e4429628df56e6075563686804' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:5.0-focal
ARG DOTNET_VERSION

ENV DOTNET_VERSION $DOTNET_VERSION

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
