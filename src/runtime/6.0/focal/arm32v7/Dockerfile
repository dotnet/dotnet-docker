ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rtm.21521.6/dotnet-runtime-6.0.0-linux-arm.tar.gz \
    && dotnet_sha512='d5f57d29d2f3073bdf4cce5cb5e6f41761a5b02ba1c71b043e83707b25e86c04fa5c6d1920e98ceef34fa69ae84564606dbeb3571789e17b9966d11d8ec94dc3' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-focal-arm32v7

ENV DOTNET_VERSION=6.0.0

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
