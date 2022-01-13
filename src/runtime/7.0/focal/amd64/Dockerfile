ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:focal-curl as installer

# Retrieve .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/7.0.0-alpha.1.22061.11/dotnet-runtime-7.0.0-alpha.1.22061.11-linux-x64.tar.gz \
    && dotnet_sha512='f219370ff63c46a60e29080b0f5dd75c95df4051fef17c1726264c67f23445fa14f8b654fd0a168add60d024ced427b21092ee859204a86b2dc08ece7779ac60' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-alpha.1-focal-amd64

ENV DOTNET_VERSION=7.0.0-alpha.1.22061.11

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
