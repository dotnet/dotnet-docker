ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rtm.21521.6/dotnet-runtime-6.0.0-rtm.21521.6-linux-arm.tar.gz \
    && dotnet_sha512='ccde266bf4ef2be738f7b810ed1f27350ba7f3ef1160e50bd21a821a219f3b6e2c471f7d3224383d331d94d3e766b835424152f949d50deb6e753098c365e92b' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-focal-arm32v7

ENV DOTNET_VERSION=6.0.0-rtm.21521.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
