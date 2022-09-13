ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=6.0.9 \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm64.tar.gz \
    && dotnet_sha512='a4ce5ec71c60690e577e96a2cd821c05d5f05b7c1754fb753353db77e938349a53d4cc55596f7384813bc44f74eac8f991a1c00cbee60483f552663cf4d8ac31' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.9-focal-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.9

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
