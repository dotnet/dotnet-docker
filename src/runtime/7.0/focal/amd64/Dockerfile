ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:focal-curl AS installer

# Retrieve .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/7.0.0-alpha.1.22053.15/dotnet-runtime-7.0.0-alpha.1.22053.15-linux-x64.tar.gz \
    && dotnet_sha512='fc8138f92840e0801db0d4d1cac6f7992103ead5f5466b5a6a72697fda5f181047a63ef58fc516090ef103279d1b6265a9ba0ef6eac468feeef37c44bc8ac57a' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-alpha.1-focal-amd64

ENV DOTNET_VERSION=7.0.0-alpha.1.22053.15

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
