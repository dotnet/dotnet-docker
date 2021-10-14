ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rtm.21513.22/dotnet-runtime-6.0.0-rtm.21513.22-linux-arm64.tar.gz \
    && dotnet_sha512='ac003ae85ac1c41e43425e496ea9743ceb71c734eaed0d14f1610869a98d0b08254a29706faf71e37662f86e6797b9777544d319caf95220474f77cd499a02cb' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-focal-arm64v8

ENV DOTNET_VERSION=6.0.0-rtm.21513.22

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
