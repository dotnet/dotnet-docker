ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.2.21470.23/dotnet-runtime-6.0.0-rc.2.21470.23-linux-arm.tar.gz \
    && dotnet_sha512='395f693ebb569a9e5ba828b05dfc316ea46ab5cbeae17258a8b736fe4799086d5c6111a6ad6d45f0e5bb580dad1e6745dfb607e83011fa252f57334716c285f3' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.2-focal-arm32v7

ENV DOTNET_VERSION=6.0.0-rc.2.21470.23

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
