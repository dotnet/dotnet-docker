ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:buster-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/5.0.10/dotnet-runtime-5.0.10-linux-x64.tar.gz \
    && dotnet_sha512='421b00d5751381e6bf829dcba8fa0d781f0efd065be492739d60a4bef2b7b362dbec77fa3289e2ee45cab40616f95318fc214699ffe2f33aa15e77c2d163841c' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:5.0.10-buster-slim-amd64

ENV DOTNET_VERSION=5.0.10

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
