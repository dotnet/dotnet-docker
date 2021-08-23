ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21421.1/dotnet-runtime-6.0.0-rc.1.21421.1-linux-x64.tar.gz \
    && dotnet_sha512='3ba9554f0ea4c3befced6821173f4d2bece930d90c1ee842d51a18c34a48932ac5946cba74d0c2c82678a5b5ff2a68e7081f0722a273931cedca804c9f57f267' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-bullseye-slim-amd64

ENV DOTNET_VERSION=6.0.0-rc.1.21421.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
