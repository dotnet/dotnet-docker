ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:focal-curl AS installer

# Retrieve .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/7.0.0-alpha.1.22053.15/dotnet-runtime-7.0.0-alpha.1.22053.15-linux-arm.tar.gz \
    && dotnet_sha512='a91d6cdf22043fb650db8dd9c9e188983de64a5aa92113271d65a532e197f2c7613707e3b49bc9c8deafcf7365e5c1610aaeb5de711fccc17071f4599af7ceaf' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-alpha.1-focal-arm32v7

ENV DOTNET_VERSION=7.0.0-alpha.1.22053.15

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
