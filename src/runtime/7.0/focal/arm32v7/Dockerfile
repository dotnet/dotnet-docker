ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer

# Retrieve .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/7.0.0-alpha.1.22060.15/dotnet-runtime-7.0.0-alpha.1.22060.15-linux-arm.tar.gz \
    && dotnet_sha512='d0d446a40f89095bfd457b16738b6d3f9268511826f5d6b2a107df58c89016deb83b3889230286ac2187030eb8c4f300aca7fca791eef1fb216e21d6c6a29af0' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-alpha.1-focal-arm32v7

ENV DOTNET_VERSION=7.0.0-alpha.1.22060.15

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
