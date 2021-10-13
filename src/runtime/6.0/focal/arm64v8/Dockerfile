ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rtm.21512.8/dotnet-runtime-6.0.0-rtm.21512.8-linux-arm64.tar.gz \
    && dotnet_sha512='2cf990ee836040e799295dd3f8b00c336736e3dbada231b3599f50ce50ceea0e468e542ad9c561d86f3202eb2b8c588f98851f78822e4cc051a420aedd6fde5c' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-focal-arm64v8

ENV DOTNET_VERSION=6.0.0-rtm.21512.8

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
