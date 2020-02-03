ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps

# Installer image
FROM arm64v8/debian:buster-slim as installer

# Installer pre-reqs
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        ca-certificates \
        curl \
    && rm -rf /var/lib/apt/lists/*

# Retrieve .NET Core
RUN dotnet_version=5.0.0-alpha.1.20072.3 \
    && curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm64.tar.gz \
    && dotnet_sha512='3e1c24b73fa5710fe2d2faa4893666a1a22f351c7064422ae34ac36d83770e42b6944e2e58a280884c1fd71fd1384bff7b8032319fa0934c6387a19230c19bfb' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET Core runtime image
FROM $REPO:5.0-buster-slim

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
