ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:buster-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/5.0.12/dotnet-runtime-5.0.12-linux-arm64.tar.gz \
    && dotnet_sha512='a8089fad8d21a4b582aa6c3d7162d56a21fee697fd400f050a772f67c2ace5e4196d1c4261d3e861d6dc2e5439666f112c406104d6271e5ab60cda80ef2ffc64' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:5.0.12-buster-slim-arm64v8

ENV DOTNET_VERSION=5.0.12

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
