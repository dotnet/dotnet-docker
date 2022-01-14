ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl AS installer

# Retrieve .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/7.0.0-alpha.1.22062.8/dotnet-runtime-7.0.0-alpha.1.22062.8-linux-arm.tar.gz \
    && dotnet_sha512='6bd856e27dc7560c9af7cc702a849bae3007c223c3b266c5b74e64efe2bb0804268947f196ea932b4c0729e8828a415eef5acc615b3ad0a24ce21d2a4a51cf9d' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-alpha.1-bullseye-slim-arm32v7

ENV DOTNET_VERSION=7.0.0-alpha.1.22062.8

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
