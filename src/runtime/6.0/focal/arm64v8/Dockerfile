ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21451.13/dotnet-runtime-6.0.0-rc.1.21451.13-linux-arm64.tar.gz \
    && dotnet_sha512='a18ab20932e36820407058ada417941535cb51c0f6c68ff8830f43c9521fdd89f31553832522bad0918779e87387ef484d2e414c07923db4ad7deecbdcbc3a52' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-focal-arm64v8

ENV DOTNET_VERSION=6.0.0-rc.1.21451.13

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
