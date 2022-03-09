ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:buster-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=5.0.15 \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='4c652a02d35bc8bafbcf15555ea319aba7c9077ddb55786cb27a11b5b16e70ef026d1d561268185e398afd16f6db1f02b4846cc83a7e0f1b0b1a07a12f84c7ee' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:5.0.15-buster-slim-amd64

# .NET Runtime version
ENV DOTNET_VERSION=5.0.15

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
