ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21421.1/dotnet-runtime-6.0.0-rc.1.21421.1-linux-arm64.tar.gz \
    && dotnet_sha512='88f965a31feeabf946cb1376b84ec7719c523f8100720f7c45ddf25a3d25cdbe17a99e260eba9f9ee6a1bf567e914a0ba938c7bc2080732b28b8bfa536391d54' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-focal-arm64v8

ENV DOTNET_VERSION=6.0.0-rc.1.21421.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
