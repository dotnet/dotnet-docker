ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21426.17/dotnet-runtime-6.0.0-rc.1.21426.17-linux-arm64.tar.gz \
    && dotnet_sha512='a9d3902af859d0d01185ced367d3b4b3efb30f6c4ef5548cbe28dcec37e0e88e582089de3994bfb40c8102724433516cb9e22814f9d1f5ab76f6aba1426908c7' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-focal-arm64v8

ENV DOTNET_VERSION=6.0.0-rc.1.21426.17

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
