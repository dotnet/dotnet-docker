ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21424.15/dotnet-runtime-6.0.0-rc.1.21424.15-linux-arm64.tar.gz \
    && dotnet_sha512='8ceee87fa5702d65e95555d4b35f0b220e9791696bf8bb2bd1eef5f7eb6909c44fd61359e2a7cdc4ddff8cf281ed6668dc1ddf68b09a321d2740864a05651956' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-focal-arm64v8

ENV DOTNET_VERSION=6.0.0-rc.1.21424.15

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
