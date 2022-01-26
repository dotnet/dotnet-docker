ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl AS installer

# Retrieve .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/7.0.0-preview.2.22074.8/dotnet-runtime-7.0.0-preview.2.22074.8-linux-arm.tar.gz \
    && dotnet_sha512='edc2a145dd5fd2d6319f18727d08a7d3c000a21dbe9b803d469f344f9bfe2f226b5fd046ffa0e544eb50de64045f7691de61c8ed8f481423bd5cef536385b60e' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-preview.2-bullseye-slim-arm32v7

ENV DOTNET_VERSION=7.0.0-preview.2.22074.8

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
