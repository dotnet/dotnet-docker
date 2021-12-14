ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:buster-curl as installer

# Retrieve .NET
RUN curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/5.0.13/dotnet-runtime-5.0.13-linux-arm.tar.gz \
    && dotnet_sha512='3d0a10b51e89d5716ab284e499e0927835d7944d7a4c15a9294679a8c4af3ca99f51cad25c0d19fa4a91b8b76a832a9bb81afb67bcdc2385730b061d135644f6' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:5.0.13-buster-slim-arm32v7

ENV DOTNET_VERSION=5.0.13

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
