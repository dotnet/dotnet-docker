ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/5.0.8/dotnet-runtime-5.0.8-linux-arm64.tar.gz \
    && dotnet_sha512='032fe4ac40fea1eeef871316da55cbfaf787a7153a0a923359cd1bcd5411b367856fb4bc3dd8c4c65d252ae4be333277e8f613966e3527866ad18f162f718c6a' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:5.0.8-focal-arm64v8

ENV DOTNET_VERSION=5.0.8

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
