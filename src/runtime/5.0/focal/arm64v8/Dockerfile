ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/5.0.7/dotnet-runtime-5.0.7-linux-arm64.tar.gz \
    && dotnet_sha512='a0c20d1530c29021fe4d483611f3f9b71a605f17b81b96aef5c07878795483ea5dae85a5e18c3ad54147a30897d389d26454298ed60d11944477ecd452c60638' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:5.0.7-focal-arm64v8

ENV DOTNET_VERSION=5.0.7

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
